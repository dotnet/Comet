using System;
using System.Collections.Generic;
using System.Diagnostics;

using System.Linq;
using System.Runtime.CompilerServices;
using Comet.Helpers;
using Comet.Internal;
//using System.Reflection;
using Comet.Reflection;
using Microsoft.Maui;
using Microsoft.Maui;
using Microsoft.Maui.Essentials;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.HotReload;
using Microsoft.Maui.Layouts;
using Microsoft.Maui.Primitives;
using Rectangle = Microsoft.Maui.Graphics.Rectangle;

namespace Comet
{

	public class View : ContextualObject, IDisposable, IView, IHotReloadableView//, IClipShapeView
	{		
		public static readonly Size UseAvailableWidthAndHeight = new Size(-1, -1);

		HashSet<(string Field, string Key)> usedEnvironmentData = new HashSet<(string Field, string Key)>();
		protected static Dictionary<string, string> HandlerPropertyMapper = new()
		{
			[nameof(MeasuredSize)] = nameof(IFrameworkElement.DesiredSize),
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
			//HotReloadHelper.ActiveViews.Add(this);
			//Debug.WriteLine($"Active View Count: {HotReloadHelper.ActiveViews.Count}");
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
		IViewHandler viewHandler;
		public IViewHandler ViewHandler
		{
			get => viewHandler;
			set
			{
				SetViewHandler(value);
			}
		}

		bool SetViewHandler(IViewHandler handler)
		{
			if (viewHandler == handler)
				return false;
			InvalidateMeasurement();
			var oldViewHandler = viewHandler;
			//viewHandler?.Remove(this);
			viewHandler = handler;
			if (replacedView != null)
				replacedView.ViewHandler = handler;
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
				ViewHandler?.SetVirtualView(view);
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
					UpdateBuiltViewContext(view);
					builtView = view.GetRenderView();
				}
			}

			/// We need to make this check if there are global views. If so, return itself so it can be in a container view
			/// If HotReload never collapse!
			/// If not collapse down to the built view.
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
			ViewHandler?.UpdateValue(GetHandlerPropertyName(property));
			replacedView?.ViewPropertyChanged(property, value);
		}

		protected virtual string GetHandlerPropertyName(string property) =>
			HandlerPropertyMapper.TryGetValue(property, out var value) ? value : property;
		

		internal override void ContextPropertyChanged(string property, object value, bool cascades)
		{
			ViewPropertyChanged(property, value);
		}

		public static void SetGlobalEnvironment(string key, object value)
		{
			Environment.SetValue(key, value, true);
			MainThread.BeginInvokeOnMainThread(() => {
				MauiHotReloadHelper.ActiveViews.OfType<View>().ForEach(x => x.ViewPropertyChanged(key, value));
			});

		}
		public static void SetGlobalEnvironment(string styleId, string key, object value)
		{
			//If there is no style, set the default key
			var typedKey = string.IsNullOrWhiteSpace(styleId) ? key : $"{styleId}.{key}";
			Environment.SetValue(typedKey, value, true);
			MainThread.BeginInvokeOnMainThread(() => {
				MauiHotReloadHelper.ActiveViews.OfType<View>().ForEach(x => x.ViewPropertyChanged(typedKey, value));
			});
		}

		public static void SetGlobalEnvironment(Type type, string key, object value)
		{
			var typedKey = ContextualObject.GetTypedKey(type, key);
			Environment.SetValue(typedKey, value, true);
			MainThread.BeginInvokeOnMainThread(() => {
				MauiHotReloadHelper.ActiveViews.OfType<View>().ForEach(x => x.ViewPropertyChanged(typedKey, value));
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

			//MauiHotReloadHelper.ActiveViews.Remove(this);

			//Debug.WriteLine($"Active View Count: {HotReloadHelper.ActiveViews.Count}");

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
				this.SetEnvironment(nameof(Frame), value, false);
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
			Parent?.InvalidateMeasurement();
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
				MeasuredSize = this.ComputeDesiredSize(availableSize.Width, availableSize.Height);
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

				// TODO ezhart If we can find reason to, we may need to add a MeasureFlags parameter to IFrameworkElement.Measure
				// Forms has and (very occasionally) uses one. I'd rather not muddle this up with it, but if it's necessary
				// we can add it. The default is MeasureFlags.None, but nearly every use of it is MeasureFlags.IncludeMargins,
				// so it's an awkward default. 

				// I'd much rather just get rid of all the uses of it which don't include the margins, and have "with margins"
				// be the default. It's more intuitive and less code to write. Also, I sort of suspect that the uses which
				// _don't_ include the margins are actually bugs.

				var frameworkElement = this as IFrameworkElement;

				widthConstraint = LayoutManager.ResolveConstraints(widthConstraint, frameworkElement.Width);
				heightConstraint = LayoutManager.ResolveConstraints(heightConstraint, frameworkElement.Height);

				MeasuredSize = GetDesiredSize(new Size(widthConstraint, heightConstraint));
				if (MeasuredSize.Width <= 0 || MeasuredSize.Height <= 0)
				{
					Console.WriteLine("Why :(");
				}
			}

			IsMeasureValid = true;
			return MeasuredSize;
		}



		public virtual void LayoutSubviews(Rectangle frame)
		{
			Frame = frame;
			if (BuiltView != null)
				BuiltView.Frame = frame;
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
			animation.Parent = new WeakReference<View>(this);
			GetAnimations(true).Add(animation);
			AnimationManger.Add(animation);
		}
		public void RemoveAnimation(Animation animation)
		{
			animation.Parent = null;
			GetAnimations(false)?.Remove(animation);
			AnimationManger.Remove(animation);
		}

		public void RemoveAnimations() => GetAnimations(false)?.ToList().ForEach(animation => {
			animations.Remove(animation);
			AnimationManger.Remove(animation);
		});

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

		bool IFrameworkElement.IsEnabled => this.GetEnvironment<bool?>(nameof(IFrameworkElement.IsEnabled)) ?? true;

		Color IFrameworkElement.BackgroundColor => this.GetBackgroundColor();

		Rectangle IFrameworkElement.Frame => Frame;

		IViewHandler IFrameworkElement.Handler
		{
			get => this.ViewHandler;
			set => SetViewHandler(value);
		}

		IFrameworkElement IFrameworkElement.Parent => this.Parent;

		Size IFrameworkElement.DesiredSize => MeasuredSize;

		protected bool IsMeasureValid;
		bool IFrameworkElement.IsMeasureValid => IsMeasureValid;

		protected bool IsArrangeValid;
		bool IFrameworkElement.IsArrangeValid => IsArrangeValid;

		double IFrameworkElement.Width => this.GetFrameConstraints()?.Width ?? -1;
		double IFrameworkElement.Height => this.GetFrameConstraints()?.Height ?? -1;

		public IView ReplacedView => this.GetView();// HasContent ? this : BuiltView ?? this;


		public bool RequiresContainer => HasContent;

		//public IShape ClipShape => this.GetClipShape();

		IView IReplaceableView.ReplacedView => this.ReplacedView;

		Thickness IView.Margin => this.GetMargin();

		string IFrameworkElement.AutomationId => this.GetAutomationId();

		//TODO: lets update these to be actual property
		FlowDirection IFrameworkElement.FlowDirection => this.GetEnvironment<FlowDirection>(nameof(IFrameworkElement.FlowDirection));

		LayoutAlignment IFrameworkElement.HorizontalLayoutAlignment => this.GetEnvironment<LayoutAlignment>(nameof(IFrameworkElement.HorizontalLayoutAlignment));

		LayoutAlignment IFrameworkElement.VerticalLayoutAlignment => this.GetEnvironment<LayoutAlignment>(nameof(IFrameworkElement.VerticalLayoutAlignment));

		void IFrameworkElement.Arrange(Rectangle bounds) => LayoutSubviews(bounds);
		Size IFrameworkElement.Measure(double widthConstraint, double heightConstraint)
			=>
			//Measure(new Size(widthConstraint, heightConstraint));
			Measure(widthConstraint, heightConstraint);
		void IFrameworkElement.InvalidateMeasure() => InvalidateMeasurement();
		void IFrameworkElement.InvalidateArrange() => IsArrangeValid = false;
		void IHotReloadableView. TransferState(IView newView) {
			var oldState = this.GetState();
			var changes = oldState.ChangedProperties;
			foreach (var change in changes)
			{
				newView.SetDeepPropertyValue(change.Key, change.Value);
			}
		}
		void IHotReloadableView.Reload() => MainThread.BeginInvokeOnMainThread(()=>Reload(true));
	}
}