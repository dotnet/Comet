using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Comet.Helpers;
using Comet.Internal;
//using System.Reflection;
using Comet.Reflection;

namespace Comet
{

    public class View : ContextualObject, IDisposable
    {
        public static readonly SizeF IllTakeWhatYouCanGive = new SizeF(-1, -1);

        internal readonly static WeakList<View> ActiveViews = new WeakList<View>();
        HashSet<(string Field, string Key)> usedEnvironmentData = new HashSet<(string Field, string Key)>();

        public event EventHandler<ViewHandlerChangedEventArgs> ViewHandlerChanged;
        public event EventHandler<EventArgs> NeedsLayout;

        public IReadOnlyList<Gesture> Gestures
        {
            get => GetPropertyFromContext<List<Gesture>>();
            internal set => SetPropertyInContext(value);
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
            HotReloadHelper.Register(this);
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
                if (viewHandler == value)
                    return;

                measurementValid = false;
                _measuredSize = SizeF.Zero;
                frame = RectangleF.Zero;
                
                var oldViewHandler = viewHandler;
                viewHandler?.Remove(this);
                viewHandler = value;
                if (replacedView != null)
                    replacedView.ViewHandler = value;
                WillUpdateView();
                viewHandler?.SetView(this.GetRenderView());
                ViewHandlerChanged?.Invoke(this, new ViewHandlerChangedEventArgs(this, oldViewHandler, value));
            }
        }

        internal void UpdateFromOldView(View view)
        {
            if (view is NavigationView nav)
            {
                ((NavigationView)this).PerformNavigate = nav.PerformNavigate;
            }
            var oldView = view.ViewHandler;
            this.Gestures = view.Gestures;
            view.ViewHandler = null;
            view.replacedView?.Dispose();
            this.ViewHandler = oldView;
        }
        View builtView;
        public View BuiltView => builtView;
        internal virtual void Reload() => ResetView();
        void ResetView()
        {
            // We save the old replaced view so we can clean it up after the diff
            var oldReplacedView = replacedView;
            //Null it out, so it isnt replaced by this.GetRenderView();
            replacedView = null;
            try
            {
                if (usedEnvironmentData.Any())
                    PopulateFromEnvironment();
                var oldView = builtView;
                builtView = null;
                
                if (ViewHandler == null)
                    return;
                ViewHandler.Remove(this);
                var view = this.GetRenderView();
                if (oldView != null)
                    view = view.Diff(oldView);
                oldView?.Dispose();
                ViewHandler?.SetView(view);
            }

            finally
            {
                //We are done, clean it up.
                if (oldReplacedView != null)
                {
                    oldReplacedView.ViewHandler = null;
                    oldReplacedView.Dispose();
                }
            }
        }

        Func<View> body;
        public Func<View> Body
        {
            get => body;
            set
            {
                body = value;
                ResetView();
             //   this.SetBindingValue(State, ref body, value, ResetPropertyString);
            }
        }

        internal View GetView() => GetRenderView();
        View replacedView;
        protected virtual View GetRenderView()
        {
            if (replacedView != null)
                return replacedView.GetRenderView();
            var replaced = HotReloadHelper.GetReplacedView(this);
            if (replaced != this)
            {
                replaced.viewThatWasReplaced = this;
                replaced.Navigation = this.Navigation;
                replaced.Parent = this;
                replaced.PopulateFromEnvironment();

                replacedView = replaced;
                return builtView = replacedView.GetRenderView();
            }
            CheckForBody();
            if (Body == null)
                return this;
            if (builtView != null)
                return builtView;
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
                return builtView = view;
            }
        }

        bool didCheckForBody;
        void CheckForBody()
        {
            if (didCheckForBody || Body != null)
                return;
            didCheckForBody = true;
            var bodyMethod = this.GetBody();
            if (bodyMethod != null)
                Body = bodyMethod;
        }

        protected virtual void WillUpdateView()
        {

        }

        internal void BindingPropertyChanged(INotifyPropertyRead bindingObject, string property, string fullProperty, object value)
        {
            var prop = property.Split('.').Last();
            if (!State.UpdateValue((bindingObject, property),fullProperty, value))
                Reload();
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
                Debug.WriteLine($"Error setting property:{property} : {value}");
                Debug.WriteLine(ex);
            }

            ViewHandler?.UpdateValue(property, value);
            replacedView?.ViewPropertyChanged(property, value);
        }

        internal override void ContextPropertyChanged(string property, object value, bool cascades)
        {
            ViewPropertyChanged(property, value);
        }

        public static async void SetGlobalEnvironment(string key, object value)
        {
            Environment.SetValue(key, value);
            await ThreadHelper.SwitchToMainThreadAsync();
            ActiveViews.ForEach(x => x.ViewPropertyChanged(key, value));
            
        }
        public static async void SetGlobalEnvironment(string styleId, string key, object value)
        {
            //If there is no style, set the default key
            var typedKey = string.IsNullOrWhiteSpace(styleId) ? key : $"{styleId}.{key}";
            Environment.SetValue(typedKey, value);
            await ThreadHelper.SwitchToMainThreadAsync();
            ActiveViews.ForEach(x => x.ViewPropertyChanged(typedKey, value));
        }

        public static async void SetGlobalEnvironment(Type type, string key, object value)
        {
            var typedKey = ContextualObject.GetTypedKey(type, key);
            Environment.SetValue(typedKey, value);
            await ThreadHelper.SwitchToMainThreadAsync();
            ActiveViews.ForEach(x => x.ViewPropertyChanged(typedKey, value));
        }

        public static void SetGlobalEnvironment(IDictionary<string, object> data)
        {
            foreach (var pair in data)
                Environment.SetValue(pair.Key, pair.Value);
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
                usedEnvironmentData.Add((f.Name,key));
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
                            usedEnvironmentData.Add((item.Field,newKey));
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
            var gestures = Gestures;
            if (gestures?.Any() ?? false)
            {
                foreach (var g in gestures)
                    ViewHandler?.UpdateValue(Gesture.RemoveGestureProperty, g);
            }
            Debug.WriteLine($"Active View Count: {ActiveViews.Count}");

            HotReloadHelper.UnRegister(this);
            var vh = ViewHandler;
            ViewHandler = null;
            //TODO: Ditch the cast
            (vh as IDisposable)?.Dispose();
            replacedView?.Dispose();
            replacedView = null;
            builtView?.Dispose();
            builtView = null;
            Body = null;
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

        private RectangleF frame;
        public virtual RectangleF Frame
        {
            get => frame;
            set
            {
                if (!frame.Equals(value))
                {
                    frame = value;
                    ViewHandler?.SetFrame(value);
                    RequestLayout();
                }
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
                    BuiltView.MeasurementValid = true;
            }
        }

        public void InvalidateMeasurement()
        {
            MeasurementValid = false;

            // TODO We should "invalidate" layout here. Close enough for now?
            frame = RectangleF.Zero;

            Parent?.InvalidateMeasurement();
            NeedsLayout?.Invoke(this, EventArgs.Empty);
        }

        private SizeF _measuredSize;
        public SizeF MeasuredSize
        {
            get => _measuredSize;
            set
            {
                _measuredSize = value;
                if (BuiltView != null)
                    BuiltView.MeasuredSize = value;
            }
        }

        public virtual SizeF Measure(SizeF availableSize)
        {
            if (BuiltView != null)
                return BuiltView.Measure(availableSize);

            var constraints = this.GetFrameConstraints();
            var width = constraints?.Width;
            var height = constraints?.Height;

            // If we have both width and height constraints, we can skip measuring the control and
            // return the constrained values.
            if (width != null && height != null)
                return new SizeF((float)width, (float)height);

            var measuredSize = viewHandler?.Measure(availableSize) ?? IllTakeWhatYouCanGive;

            // If we have a constraint for just one of the values, then combine the constrained value
            // with the measured value for our size.
            if (width != null || height != null)
                return new SizeF(width ?? measuredSize.Width, height ?? measuredSize.Height);

            return measuredSize;
        }

        protected virtual void RequestLayout()
        {
            var constraints = this.GetFrameConstraints();
            var width = constraints?.Width ?? Frame.Width;
            var height = constraints?.Height ?? Frame.Height;

            if (width > 0 && height > 0)
            {
                var margin = BuiltView?.GetMargin();
                if (margin != null)
                {
                    width -= ((Thickness) margin).HorizontalThickness;
                    height -= ((Thickness) margin).VerticalThickness;
                }
                
                if (!MeasurementValid)
                {
                    MeasuredSize = Measure(new SizeF(width, height));
                    MeasurementValid = true;
                }

                Layout();
            }
        }

        private void Layout()
        {
            var width = Frame.Width;
            var height = Frame.Height;

            var constraints = this.GetFrameConstraints();
            var alignment = constraints?.Alignment ?? Alignment.Center;
            var xFactor = .5f;
            switch (alignment.Horizontal)
            {
                case HorizontalAlignment.Leading:
                    xFactor = 0;
                    break;
                case HorizontalAlignment.Trailing:
                    xFactor = 1;
                    break;
            }

            var yFactor = .5f;
            switch (alignment.Vertical)
            {            
                case VerticalAlignment.Bottom:
                    yFactor = 1;
                    break;
                case VerticalAlignment.Top:
                    yFactor = 0;
                    break;                
            }

            // Make sure the final width is not larger than the frame we're allocated. 
            var finalWidth = Math.Min(width, MeasuredSize.Width);
            var finalHeight = Math.Min(height, MeasuredSize.Height);
            
            var x = (width - finalWidth) * xFactor;
            var y = (height - finalHeight) * yFactor;

            LayoutSubviews(new RectangleF(Frame.X + x,Frame.Y+y,finalWidth,finalHeight));
        }

        public virtual void LayoutSubviews(RectangleF frame)
        {
            if (BuiltView != null)
                BuiltView.Frame = frame;
        }
        public override string ToString() => $"{this.GetType()} - {this.Id}";

        View notificationView => replacedView ?? builtView;

        public virtual void ViewDidAppear()
        {
            notificationView?.ViewDidAppear();
        }
        public virtual void ViewDidDisappear()
        {
            notificationView?.ViewDidDisappear();
        }
    }
}
