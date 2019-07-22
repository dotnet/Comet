using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HotUI.Helpers;
using HotUI.Internal;
//using System.Reflection;
using HotUI.Reflection;

namespace HotUI
{

    public class View : ContextualObject, IDisposable
    {
        internal readonly static WeakList<View> ActiveViews = new WeakList<View>();
        HashSet<string> usedEnvironmentData = new HashSet<string>();

        public event EventHandler<ViewHandlerChangedEventArgs> ViewHandlerChanged;
        public event EventHandler<EventArgs> NeedsLayout;

        View parent;
        string id;

        public string Id
        {
            get => id ?? (id = Guid.NewGuid().ToString());
            set => id = value;
        }

        string tag;
        public string Tag
        {
            get => tag;
            internal set => tag = value;
        }

        public View Parent
        {
            get => parent;
            set
            {
                if (parent == value)
                    return;
                parent = value;
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
        protected State State { get; set; }
        internal State GetState() => State;

        public View(bool hasConstructors)
        {
            ActiveViews.Add(this);
            Debug.WriteLine($"Active View Count: {ActiveViews.Count}");
            HotReloadHelper.Register(this);
            State = StateBuilder.CurrentState ?? new State
            {
                StateChanged = ResetView
            };
            SetEnvironmentFields();
            if (!hasConstructors)
                State.StartBuildingView();

        }
        public View() : this(false)
        {

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
                measuredSize = SizeF.Zero;
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
            view.ViewHandler = null;
            view.replacedView?.Dispose();
            this.ViewHandler = oldView;
        }
        View builtView;
        public View BuiltView => builtView;
        internal void Reload() => ResetView();
        void ResetView()
        {
            if (usedEnvironmentData.Any())
                SetEnvironmentFields();
            var oldView = builtView;
            builtView = null;
            if (replacedView != null)
            {
                replacedView.ViewHandler = null;
                replacedView.Dispose();
                replacedView = null;
            }
            if (ViewHandler == null)
                return;
            ViewHandler.Remove(this);
            var view = this.GetRenderView();
            if (oldView != null)
                view = view.Diff(oldView);
            oldView?.Dispose();
            ViewHandler?.SetView(view);
        }

        Func<View> body;
        public Func<View> Body
        {
            get => body;
            set => this.SetValue(State, ref body, value, ResetPropertyString);
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
                replaced.Parent = this.Parent ?? this;
                replacedView = replaced;
                replacedView.ViewHandler = ViewHandler;
                return builtView = replacedView.GetRenderView();
            }
            CheckForBody();
            if (Body == null)
                return this;
            if (builtView != null)
                return builtView;
            Debug.WriteLine($"Building View: {this.GetType().Name}");
            using (new StateBuilder(State))
            {
                State.SetParent(this);
                State.StartProperty();
                var view = Body.Invoke();
                view.Parent = this;
                if (view is NavigationView navigationView)
                    Navigation = navigationView;
                var props = State.EndProperty();
                var propCount = props.Length;
                if (propCount > 0)
                {
                    State.BindingState.AddViewProperty(props, this, ResetPropertyString);
                }
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

        internal void BindingPropertyChanged(string property, object value)
        {

            ViewPropertyChanged(property, value);
        }
        protected const string ResetPropertyString = "ResetPropertyString";
        protected virtual void ViewPropertyChanged(string property, object value)
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

        internal override void ContextPropertyChanged(string property, object value)
        {
            ViewPropertyChanged(property, value);
        }

        public static void SetGlobalEnvironment(string key, object value)
        {
            Environment.SetValue(key, value);
            Device.InvokeOnMainThread(() =>
            {
                ActiveViews.ForEach(x => x.ViewPropertyChanged(key, value));
            });
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
                var value = this.GetEnvironment(key);
                State.BindingState.AddGlobalProperty(key);
                usedEnvironmentData.Add(key);
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
                        key = key.FirstCharToUpper();
                        value = this.GetEnvironment(key);
                        if (value != null)
                        {
                            usedEnvironmentData.Add(key);
                            State.BindingState.AddGlobalProperty(key);
                        }
                    }
                }
                if (value == null && viewThatWasReplaced != null)
                {
                    value = viewThatWasReplaced.GetEnvironment(key);
                }
                if (value != null)
                    f.SetValue(this, value);
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
            Context.Clear();
            State?.DisposingObject(this);
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

        Thickness padding = Thickness.Empty;
        public Thickness Padding
        {
            get => padding;
            internal set => this.SetValue(State, ref padding, value, ResetPropertyString);
        }

        FrameConstraints frameConstraints;
        public FrameConstraints FrameConstraints
        {
            get => BuiltView?.FrameConstraints ?? frameConstraints;
            internal set => this.SetValue(State, ref frameConstraints, value, ResetPropertyString);
        }
        
        object layoutConstraints;
        public object LayoutConstraints
        {
            get => layoutConstraints;
            internal set => this.SetValue(State, ref layoutConstraints, value, ResetPropertyString);
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
            Parent?.InvalidateMeasurement();
            NeedsLayout?.Invoke(this, EventArgs.Empty);
        }

        private SizeF measuredSize;
        public SizeF MeasuredSize
        {
            get => measuredSize;
            set
            {
                measuredSize = value;
                if (BuiltView != null)
                    BuiltView.MeasuredSize = value;
            }
        }

        public virtual SizeF Measure(SizeF availableSize)
        {
            if (BuiltView != null)
                return BuiltView.Measure(availableSize);

            var width = frameConstraints?.Width;
            var height = frameConstraints?.Height;

            // If we have both width and height constraints, we can skip measuring the control and
            // return the constrained values.
            if (width != null && height != null)
                return new SizeF((float)width, (float)height);

            var measuredSize = viewHandler?.Measure(availableSize) ?? availableSize;

            // If we have a constraint for just one of the values, then combine the constrained value
            // with the measured value for our size.
            if (width != null || height != null)
                return new SizeF(width ?? measuredSize.Width, height ?? measuredSize.Height);

            return measuredSize;
        }
        
        protected virtual void RequestLayout()
        {
            var width = FrameConstraints?.Width ?? Frame.Width;
            var height = FrameConstraints?.Height ?? Frame.Height;

            if (width > 0 && height > 0)
            {
                var padding = BuiltView?.GetPadding();
                if (padding != null)
                {
                    width -= ((Thickness) padding).HorizontalThickness;
                    height -= ((Thickness) padding).VerticalThickness;
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

            var alignment = FrameConstraints?.Alignment ?? Alignment.Center;
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

            var x = (width - MeasuredSize.Width) * xFactor;
            var y = (height - MeasuredSize.Height) * yFactor;

            LayoutSubviews(new RectangleF(Frame.X + x,Frame.Y+y,MeasuredSize.Width,MeasuredSize.Height));
        }

        public virtual void LayoutSubviews(RectangleF bounds)
        {
            if (BuiltView != null)
                BuiltView.Frame = bounds;
        }
    }
}
