using System;
using System.Collections.Generic;
using System.Diagnostics;

using System.Linq;
using System.Runtime.CompilerServices;
using Comet.Helpers;
using Comet.Internal;
//using System.Reflection;
using Comet.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Animations;
using Microsoft.Maui.Essentials;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.HotReload;
using Microsoft.Maui.Internal;
using Microsoft.Maui.Layouts;
using Microsoft.Maui.Primitives;
using Rectangle = Microsoft.Maui.Graphics.Rectangle;

namespace Comet
{

	public class View : ContextualObject, IDisposable, IView, IHotReloadableView,ISafeAreaView, IContentTypeHash, IAnimator, ITitledElement
	{

		static internal readonly WeakList<IView> ActiveViews = new WeakList<IView>();
		HashSet<(string Field, string Key)> usedEnvironmentData = new HashSet<(string Field, string Key)>();
		protected static Dictionary<string, string> HandlerPropertyMapper = new()
		{
			[nameof(MeasuredSize)] = nameof(IView.DesiredSize),
			[EnvironmentKeys.Fonts.Size] = nameof(IText.Font),
			[EnvironmentKeys.Fonts.Slant] = nameof(IText.Font),
			[EnvironmentKeys.Fonts.Family] = nameof(IText.Font),
			[EnvironmentKeys.Fonts.Weight] = nameof(IText.Font),
		};

		IReloadHandler reloadHandler;
		public IReloadHandler ReloadHandler
		{
			get => reloadHandler;
			set
			{
				reloadHandler = value;
			}
		}
		WeakReference parent;

		public string Id { get; } = IDGenerator.Instance.Next;

		public string Tag
		{
			get => GetPropertyFromContext<string>();
			internal set => SetPropertyInContext(value);
		}

		internal T GetPropertyFromContext<T>([CallerMemberName] string property = null) => this.GetEnvironment<T>(property, false);
		internal void SetPropertyInContext(object value, [CallerMemberName] string property = null)
		{
			if (this.IsDisposed)
				return;
			this.SetEnvironment(property, value, false);
		}

		public View Parent
		{
			get => parent?.Target as View;
			set
			{
				var p = parent?.Target as View;
				if (p == value)
					return;
				parent = new WeakReference(value);
				OnParentChange(value);
			}
		}
		internal void UpdateNavigation()
		{
			OnParentChange(Navigation);
		}
		protected virtual void OnParentChange(View parent)
		{
			this.Navigation = parent.Navigation ?? parent as NavigationView;
		}
		public NavigationView Navigation { get; set; }
		protected BindingState State { get; set; }
		internal BindingState GetState() => State;

		public View()
		{
			ActiveViews.Add(this);
			Debug.WriteLine($"Active View Count: {ActiveViews.Count}");
			//HotReloadHelper.Register(this);
			//TODO: Should this need its view?
			State = new BindingState();
			StateManager.ConstructingView(this);
			SetEnvironmentFields();

		}

		WeakReference __viewThatWasReplaced;
		View viewThatWasReplaced
		{
			get => __viewThatWasReplaced?.Target as View;
			set => __viewThatWasReplaced = new WeakReference(value);
		}
		public string AccessibilityId { get; set; }
		IElementHandler viewHandler;
		public IElementHandler ViewHandler
		{
			get => viewHandler;
			set
			{
				SetViewHandler(value);
			}
		}

		bool SetViewHandler(IElementHandler handler)
		{
			if (viewHandler == handler)
				return false;
			InvalidateMeasurement();
			var oldViewHandler = viewHandler;
			//viewHandler?.Remove(this);
			viewHandler = handler;
			if(viewHandler?.VirtualView != this)
				viewHandler?.SetVirtualView(this);
			if (replacedView != null)
				replacedView.ViewHandler = handler;
			AddAllAnimationsToManager();
			return true;

		}

		internal void UpdateFromOldView(View view)
		{
			if (view is NavigationView nav)
			{
				((NavigationView)this).SetPerformNavigate(nav);
				((NavigationView)this).SetPerformPop(nav);

			}
			var oldView = view.ViewHandler;
			this.ReloadHandler = view.ReloadHandler;
			view.ViewHandler = null;
			view.replacedView?.Dispose();
			this.ViewHandler = oldView;
		}
		View builtView;
		public View BuiltView => builtView?.BuiltView ?? builtView;
		internal virtual void Reload(bool isHotReload) => ResetView(isHotReload);
		void ResetView(bool isHotReload = false)
		{
			// We save the old replaced view so we can clean it up after the diff
			var oldReplacedView = replacedView;
			try
			{
				if (usedEnvironmentData.Any())
					PopulateFromEnvironment();
				//Built view shows off the view that has the Handler, But we still need to dispose the parent!
				var oldView = BuiltView;
				var oldParentView = builtView;
				builtView = null;
				//Null it out, so it isnt replaced by this.GetRenderView();
				replacedView = null;

				//if (ViewHandler == null)
				//	return;
				//ViewHandler?.Remove(this);
				var view = this.GetRenderView();
				if (oldView != null)
					view = view.Diff(oldView, isHotReload);
				if (view != oldView)
					oldView?.Dispose();
				if (view != oldParentView)
					oldParentView?.Dispose();
				animations?.ForEach(x => x.Dispose());
				ViewHandler?.SetVirtualView(this);
				ReloadHandler?.Reload();
			}
			finally
			{
				//We are done, clean it up.
				if (oldReplacedView != null)
				{
					oldReplacedView.ViewHandler = null;
					oldReplacedView.Dispose();
				}
				InvalidateMeasurement();
			}
		}

		Func<View> body;
		public Func<View> Body
		{
			get => body;
			set
			{
				var wasSet = body != null;
				body = value;
				if (wasSet)
					ResetView();
				//   this.SetBindingValue(State, ref body, value, ResetPropertyString);
			}
		}

		///
		public bool HasContent => Body != null && (MauiHotReloadHelper.IsEnabled || hasGlobalState);

		bool hasGlobalState => State.GlobalProperties.Any();
		internal View GetView() => GetRenderView();
		View replacedView;
		protected virtual View GetRenderView()
		{
			if (replacedView != null)
				return replacedView.GetRenderView();
			var replaced = MauiHotReloadHelper.GetReplacedView(this) as View;
			if (replaced != this)
			{
				replaced.viewThatWasReplaced = this;
				replaced.ViewHandler = ViewHandler;
				replaced.Navigation = this.Navigation;
				replaced.Parent = this;
				replaced.ReloadHandler = this.ReloadHandler;
				replaced.PopulateFromEnvironment();

				replacedView = replaced;
				return builtView = replacedView.GetRenderView();
			}
			CheckForBody();
			if (Body == null)
				return this;


			if (BuiltView == null)
			{
				Debug.WriteLine($"Building View: {this.GetType().Name}");
				using (new StateBuilder(this))
				{
					try
					{
						var view = Body.Invoke();
						view.Parent = this;
						if (view is NavigationView navigationView)
							Navigation = navigationView;
						var props = StateManager.EndProperty();
						var propCount = props.Count;
						if (propCount > 0)
						{
							State.AddGlobalProperties(props);
						}
						builtView = view.GetRenderView();
						UpdateBuiltViewContext(builtView);
					}
					catch(Exception ex)
					{
						if (Debugger.IsAttached)
						{
							builtView = new VStack {new Text(ex.Message.ToString()).LineBreakMode(LineBreakMode.WordWrap) };
						}
						else throw ex;
					}
				}
			}

			// We need to make this check if there are global views. If so, return itself so it can be in a container view
			// If HotReload never collapse!
			// If not collapse down to the built view.
			//return HotReloadHelper.IsEnabled || hasGlobalState ? this : BuiltView;
			return BuiltView;
		}

		bool didCheckForBody;
		void CheckForBody()
		{
			if (didCheckForBody)
				return;
			if (usedEnvironmentData.Any())
				PopulateFromEnvironment();
			StateManager.CheckBody(this);
			didCheckForBody = true;
			if (Body != null)
				return;
			var bodyMethod = this.GetBody();
			if (bodyMethod != null)
				Body = bodyMethod;
		}

		internal void BindingPropertyChanged(INotifyPropertyRead bindingObject, string property, string fullProperty, object value)
		{
			var prop = property.Split('.').Last();
			if (!State.UpdateValue(this, (bindingObject, property), fullProperty, value))
				Reload(false);
			else
				ViewPropertyChanged(prop, value);
		}
		protected const string ResetPropertyString = "ResetPropertyString";
		public virtual void ViewPropertyChanged(string property, object value)
		{
			if (property == ResetPropertyString)
			{
				ResetView();
				return;
			}

			try
			{
				this.SetPropertyValue(property, value);
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Error setting property:{property} : {value} on :{this}");
				Debug.WriteLine(ex);
			}
			var newPropName = GetHandlerPropertyName(property);
			ViewHandler?.UpdateValue(newPropName);
			builtView?.ViewPropertyChanged(property, value);
		}

		protected virtual string GetHandlerPropertyName(string property) =>
			HandlerPropertyMapper.TryGetValue(property, out var value) ? value : property;
		

		internal override void ContextPropertyChanged(string property, object value, bool cascades)
		{
			builtView?.ContextPropertyChanged(property, value, cascades);
			ViewPropertyChanged(property, value);
		}

		public static void SetGlobalEnvironment(string key, object value)
		{
			Environment.SetValue(key, value, true);
			ThreadHelper.RunOnMainThread(() => {
				ActiveViews.OfType<View>().ForEach(x => x.ViewPropertyChanged(key, value));
			});

		}
		public static void SetGlobalEnvironment(string styleId, string key, object value)
		{
			//If there is no style, set the default key
			var typedKey = string.IsNullOrWhiteSpace(styleId) ? key : $"{styleId}.{key}";
			Environment.SetValue(typedKey, value, true);
			ThreadHelper.RunOnMainThread(() => {
				ActiveViews.OfType<View>().ForEach(x => x.ViewPropertyChanged(typedKey, value));
			});
		}

		public static void SetGlobalEnvironment(Type type, string key, object value)
		{
			var typedKey = ContextualObject.GetTypedKey(type, key);
			Environment.SetValue(typedKey, value, true);
			ThreadHelper.RunOnMainThread(() => {
				ActiveViews.OfType<View>().ForEach(x => x.ViewPropertyChanged(typedKey, value));
			});
		}

		public static void SetGlobalEnvironment(IDictionary<string, object> data)
		{
			foreach (var pair in data)
				Environment.SetValue(pair.Key, pair.Value, true);
		}
		public static T GetGlobalEnvironment<T>(string key) => Environment.GetValue<T>(key);

		void SetEnvironmentFields()
		{
			var fields = this.GetFieldsWithAttribute(typeof(EnvironmentAttribute));
			if (!fields.Any())
				return;
			foreach (var f in fields)
			{
				var attribute = f.GetCustomAttributes(true).OfType<EnvironmentAttribute>().FirstOrDefault();
				var key = attribute.Key ?? f.Name;
				usedEnvironmentData.Add((f.Name, key));
				State.AddGlobalProperty((View.Environment, key));
			}
		}
		void PopulateFromEnvironment()
		{
			var keys = usedEnvironmentData.ToList();
			foreach (var item in keys)
			{
				var key = item.Key;
				var value = this.GetEnvironment(key);
				if(value == null)
				{
					//Get the current MauiContext
					//I might be able to do something better, like searching up though the parent
					//Maybe I can do something where I get the current Context whenever I build
					var mauiContext = this.ViewHandler?.MauiContext ?? CometApp.CurrentWindow?.MauiContext;
					if (mauiContext != null)
					{
						var type = this.GetType();
						var prop = type.GetDeepField(item.Field);
						var service = mauiContext.Services.GetService(prop.FieldType);
						if (service != null)
							value = service;
					}
				}
				if (value == null)
				{
					//Check the replaced view
					if (viewThatWasReplaced != null)
					{
						value = viewThatWasReplaced.GetEnvironment(key);
					}
					if (value == null)
					{
						//Lets try again with first letter uppercased;
						var newKey = key.FirstCharToUpper();
						value = this.GetEnvironment(newKey);
						if (value != null)
						{
							key = newKey;
							usedEnvironmentData.Remove(item);
							usedEnvironmentData.Add((item.Field, newKey));
						}
					}
				}
				if (value == null && viewThatWasReplaced != null)
				{
					value = viewThatWasReplaced.GetEnvironment(item.Key);
				}
				if (value != null)
				{
					StateManager.ListenToEnvironment(this);
					State.AddGlobalProperty((View.Environment, key));
					if (value is INotifyPropertyRead notify)
						StateManager.RegisterChild(this, notify, key);
					this.SetDeepPropertyValue(item.Field, value);
				}
			}
		}
		public bool IsDisposed => disposedValue;
		bool disposedValue = false;
		protected virtual void Dispose(bool disposing)
		{
			if (!disposing)
				return;

			ActiveViews.Remove(this);

			Debug.WriteLine($"Active View Count: {ActiveViews.Count}");

			MauiHotReloadHelper.UnRegister(this);
			var vh = ViewHandler;
			ViewHandler = null;
			//TODO: Ditch the cast
			(vh as IDisposable)?.Dispose();
			replacedView?.Dispose();
			replacedView = null;
			builtView?.Dispose();
			builtView = null;
			body = null;
			Context(false)?.Clear();
			StateManager.Disposing(this);
			State.Clear();
			State = null;

		}
		void OnDispose(bool disposing)
		{
			if (disposedValue)
				return;
			disposedValue = true;
			Dispose(disposing);
		}

		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			OnDispose(true);
		}

		public virtual Rectangle Frame
		{
			get => this.GetEnvironment<Rectangle?>(nameof(Frame), false) ?? Rectangle.Zero;
			set
			{
				var f = Frame;
				if (f == value)
					return;
				this.SetEnvironment(nameof(Frame), value, false);
				(ViewHandler as IViewHandler)?.NativeArrange(value);
			}
		}


		private bool measurementValid;
		public bool MeasurementValid
		{
			get => measurementValid;
			set
			{
				measurementValid = value;
				if (BuiltView != null)
					BuiltView.MeasurementValid = value;
			}
		}

		public void InvalidateMeasurement()
		{
			MeasurementValid = false;
			(Parent as IView)?.InvalidateMeasure();
		}

		private Size _measuredSize;
		public Size MeasuredSize
		{
			get => _measuredSize;
			set
			{
				_measuredSize = value;
				if (BuiltView != null)
					BuiltView.MeasuredSize = value;
			}
		}
		public virtual Size GetDesiredSize(Size availableSize)
		{
			if (BuiltView != null)
				return BuiltView.GetDesiredSize(availableSize);
			if (!IsMeasureValid)
			{
				var fe = (IView)this;
				var ms = this.ComputeDesiredSize(availableSize.Width, availableSize.Height);
				if(fe.Width > 0)
					ms.Width = fe.Width;
				if (fe.Height > 0)
					ms.Height = fe.Height;
				//TODO: Remove this when we get some LayoutOptions...
				//This check ignores MArgin which is bad
				var hSizing = this.GetHorizontalLayoutAlignment(this.Parent as ContainerView);
				var vSizing = this.GetVerticalLayoutAlignment(this.Parent as ContainerView);
				if (hSizing == LayoutAlignment.Fill && !double.IsInfinity(availableSize.Width))
				{
					ms.Width = availableSize.Width;
				}
				if (vSizing == LayoutAlignment.Fill && !double.IsInfinity(availableSize.Height))
				{
					ms.Height = availableSize.Height;
				}

				ms.Width = Math.Min(ms.Width, availableSize.Width);
				ms.Height = Math.Min(ms.Height, availableSize.Height);
				MeasuredSize = ms;
			}
			IsMeasureValid = true;
			return MeasuredSize;
		}


		public Size Measure(double widthConstraint, double heightConstraint)
		{

			if (BuiltView != null)
				return MeasuredSize = BuiltView.Measure(widthConstraint, heightConstraint);

			if (!IsMeasureValid)
			{
				// TODO ezhart Adjust constraints to account for margins

				// TODO ezhart If we can find reason to, we may need to add a MeasureFlags parameter to IView.Measure
				// Forms has and (very occasionally) uses one. I'd rather not muddle this up with it, but if it's necessary
				// we can add it. The default is MeasureFlags.None, but nearly every use of it is MeasureFlags.IncludeMargins,
				// so it's an awkward default. 

				// I'd much rather just get rid of all the uses of it which don't include the margins, and have "with margins"
				// be the default. It's more intuitive and less code to write. Also, I sort of suspect that the uses which
				// _don't_ include the margins are actually bugs.

				var frameworkElement = this as IView;
				
				var size = GetDesiredSize(new Size(widthConstraint, heightConstraint));
				widthConstraint = LayoutManager.ResolveConstraints(widthConstraint, frameworkElement.Width, size.Width);
				heightConstraint = LayoutManager.ResolveConstraints(heightConstraint, frameworkElement.Height, size.Height);

				MeasuredSize = new Size(widthConstraint,heightConstraint);
				if (MeasuredSize.Width <= 0 || MeasuredSize.Height <= 0)
				{
					Console.WriteLine($"Why :( - {this}");
				}
			}

			IsMeasureValid = true;
			return MeasuredSize;
		}



		public virtual void LayoutSubviews(Rectangle frame)
		{
			Frame = frame;
			if (BuiltView != null)
				BuiltView.LayoutSubviews(frame);
		}
		public override string ToString() => $"{this.GetType()} - {this.Id}";

		View notificationView => replacedView ?? BuiltView;

		public virtual void ViewDidAppear()
		{
			notificationView?.ViewDidAppear();
			ResumeAnimations();
		}
		public virtual void ViewDidDisappear()
		{
			notificationView?.ViewDidDisappear();
			PauseAnimations();
		}

		List<Animation> animations;
		List<Animation> GetAnimations(bool create) => !create ? animations : animations ?? (animations = new List<Animation>());
		public List<Animation> Animations => animations;

		public void AddAnimation(Animation animation)
		{
			animation.Parent = new WeakReference<IAnimator>(this);
			GetAnimations(true).Add(animation);
			AddAnimationsToManager(animation);
		}
		public void RemoveAnimation(Animation animation)
		{
			animation.Parent = null;
			GetAnimations(false)?.Remove(animation);
		}

		public void RemoveAnimations() => GetAnimations(false)?.ToList().ForEach(animation => {
			animations.Remove(animation);
			RemoveAnimationsFromManager(animation);
		});
		void AddAnimationsToManager(Animation animation)
		{
			var animationManager = GetAnimationManager();
			if (animationManager == null)
				return;
			ThreadHelper.RunOnMainThread(()=> animationManager.Add(animation));
		}

		protected virtual IMauiContext GetMauiContext() => ViewHandler?.MauiContext ?? BuiltView?.GetMauiContext();
		IAnimationManager GetAnimationManager() => GetMauiContext()?.Services.GetRequiredService<IAnimationManager>();

		void AddAllAnimationsToManager()
		{
			var animationManager = GetAnimationManager();
			if (animationManager == null)
				return;
			ThreadHelper.RunOnMainThread(()=>GetAnimations(false)?.ToList().ForEach(animationManager.Add));
		}
		void RemoveAnimationsFromManager(Animation animation)
		{
			var animationManager = GetAnimationManager();
			if (animationManager == null)
				return;
			animationManager.Remove(animation);
		}

		public virtual void PauseAnimations()
		{
			GetAnimations(false)?.ForEach(x => x.Pause());
			notificationView?.PauseAnimations();
		}
		public virtual void ResumeAnimations()
		{
			GetAnimations(false)?.ForEach(x => x.Resume());
			notificationView?.ResumeAnimations();
		}

		bool IView.IsEnabled => this.GetEnvironment<bool?>(nameof(IView.IsEnabled)) ?? true;

		Rectangle IView.Frame
		{
			get => Frame;
			set => Frame = value;
		}

		IViewHandler IView.Handler
		{
			get => (ViewHandler as IViewHandler);
			set => SetViewHandler(value);
		}

		IElementHandler IElement.Handler
		{
			get => this.ViewHandler;
			set => SetViewHandler(value);
		}

		IElement IElement.Parent => this.Parent;

		Size IView.DesiredSize => MeasuredSize;

		protected bool IsMeasureValid;
		//bool IView.IsMeasureValid => IsMeasureValid;

		protected bool IsArrangeValid;
		//bool IView.IsArrangeValid => IsArrangeValid;

		double IView.Width => this.GetFrameConstraints()?.Width ?? Dimension.Unset;
		double IView.Height => this.GetFrameConstraints()?.Height ?? Dimension.Unset;

		double IView.MinimumHeight => this.GetEnvironment<double?>(nameof(IView.MinimumHeight)) ?? Dimension.Unset;
		double IView.MaximumWidth => this.GetEnvironment<double?>(nameof(IView.MaximumWidth)) ?? Dimension.Maximum;
		double IView.MinimumWidth => this.GetEnvironment<double?>(nameof(IView.MinimumWidth)) ?? Dimension.Unset;
		double IView.MaximumHeight => this.GetEnvironment<double?>(nameof(IView.MaximumHeight)) ?? Dimension.Maximum;

		public IView ReplacedView => this.GetView();// HasContent ? this : BuiltView ?? this;


		public bool RequiresContainer => HasContent;

		IShape IView.Clip => this.GetClipShape();

		IView IReplaceableView.ReplacedView => this.ReplacedView;

		Thickness IView.Margin => this.GetMargin();

		string IView.AutomationId => this.GetAutomationId();

		//TODO: lets update these to be actual property
		FlowDirection IView.FlowDirection => this.GetEnvironment<FlowDirection>(nameof(IView.FlowDirection));

		LayoutAlignment IView.HorizontalLayoutAlignment => this.GetHorizontalLayoutAlignment(this.Parent as ContainerView);

		LayoutAlignment IView.VerticalLayoutAlignment => this.GetVerticalLayoutAlignment(this.Parent as ContainerView);

		Semantics IView.Semantics => this.GetEnvironment<Semantics>(nameof(IView.Semantics));

		bool ISafeAreaView.IgnoreSafeArea => this.GetIgnoreSafeArea(false);

		Visibility IView.Visibility => Visibility.Visible;

		double IView.Opacity => this.GetOpacity();

		Paint IView.Background => this.GetBackground();

		double ITransform.TranslationX => this.GetEnvironment<double>(nameof(ITransform.TranslationX));

		double ITransform.TranslationY => this.GetEnvironment<double>(nameof(ITransform.TranslationY));

		double ITransform.Scale => this.GetEnvironment<double?>(nameof(ITransform.Scale)) ?? 1;

		double ITransform.ScaleX => this.GetEnvironment<double?>(nameof(ITransform.ScaleX)) ?? 1;

		double ITransform.ScaleY => this.GetEnvironment<double?>(nameof(ITransform.ScaleY)) ?? 1;

		double ITransform.Rotation => this.GetEnvironment<double>(nameof(ITransform.Rotation));

		double ITransform.RotationX => this.GetEnvironment<double>(nameof(ITransform.RotationX));

		double ITransform.RotationY => this.GetEnvironment<double>(nameof(ITransform.RotationY));

		double ITransform.AnchorX => this.GetEnvironment<double?>(nameof(ITransform.AnchorX)) ?? .5;

		double ITransform.AnchorY => this.GetEnvironment<double?>(nameof(ITransform.AnchorY)) ?? .5;


		public string Title => this.GetTitle();

		IShadow IView.Shadow => this.GetEnvironment<IShadow>(nameof(IView.Shadow));

		//int IView.ZIndex => this.GetEnvironment<int?>(nameof(IView.ZIndex)) ?? 0;

		Size IView.Arrange(Rectangle bounds)
		{
			LayoutSubviews(bounds);
			return Frame.Size;
		}
		Size IView.Measure(double widthConstraint, double heightConstraint)
			=>
			//Measure(new Size(widthConstraint, heightConstraint));
			Measure(widthConstraint, heightConstraint);
		void IView.InvalidateMeasure() => InvalidateMeasurement();
		void IView.InvalidateArrange() => IsArrangeValid = false;
		void IHotReloadableView. TransferState(IView newView) {
			var oldState = this.GetState();
			if (oldState == null)
				return;
			var changes = oldState.ChangedProperties;
			foreach (var change in changes)
			{
				newView.SetDeepPropertyValue(change.Key, change.Value);
			}
		}
		void IHotReloadableView.Reload() => ThreadHelper.RunOnMainThread(() => Reload(true));
		protected int? TypeHashCode;
		public virtual int GetContentTypeHashCode() => this.replacedView?.GetContentTypeHashCode() ?? (TypeHashCode ??= this.GetType().GetHashCode());
	}
}