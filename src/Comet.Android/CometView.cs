using System;
using AContext = Android.Content.Context;
using AView = Android.Views.View;
using AViewGroup = Android.Views.ViewGroup;

namespace Comet.Android
{
    public class CometView : AViewGroup
    {
        private View _virtualView;
        private AndroidViewHandler _handler;
        private AView _nativeView;

        public CometView(AContext context) : base(context)
        {
        }

        public View CurrentView
        {
            get => _virtualView;
            set
            {
                if (value == _virtualView)
                    return;

                if (_virtualView != null)
                {
                    _virtualView.ViewHandlerChanged -= HandleViewHandlerChanged;
                    _virtualView.NeedsLayout -= HandleNeedsLayout;
                    if (_handler is AndroidViewHandler viewHandler)
                        viewHandler.NativeViewChanged -= HandleNativeViewChanged;
                }

                _virtualView = value;

                if (_virtualView != null)
                {
                    _handler = _virtualView.GetOrCreateViewHandler();
                    
                    _virtualView.ViewHandlerChanged += HandleViewHandlerChanged;
                    _virtualView.NeedsLayout += HandleNeedsLayout;
                    if (_handler is AndroidViewHandler viewHandler)
                        viewHandler.NativeViewChanged += HandleNativeViewChanged;
                    
                    HandleNativeViewChanged(this, new ViewChangedEventArgs(_virtualView, null, (AView)_handler.NativeView));
                }
            }
        }
        
        private void HandleNeedsLayout(object sender, EventArgs e)
        {
            this.RequestLayout();
        }

        private void HandleViewHandlerChanged(object sender, ViewHandlerChangedEventArgs e)
        {
            Console.WriteLine($"[{GetType().Name}] HandleViewHandlerChanged: [{sender.GetType()}] From:[{e.OldViewHandler?.GetType()}] To:[{e.NewViewHandler?.GetType()}]");

            if (e.OldViewHandler is AndroidViewHandler oldHandler)
            {
                oldHandler.NativeViewChanged -= HandleNativeViewChanged;
                this.RemoveView(_nativeView);
            }

            if (e.NewViewHandler is AndroidViewHandler newHandler)
            {
                newHandler.NativeViewChanged += HandleNativeViewChanged;
                _nativeView = newHandler.View ?? new AView(AndroidContext.CurrentContext);
                AddView(_nativeView, this.Width, this.Height);
                this.RequestLayout();
            }
        }

        void HandleNativeViewChanged(object sender, ViewChangedEventArgs args)
        {
            if (_virtualView == null)
                return;

            var newNativeView = _handler?.View;
            if (newNativeView == _nativeView)
                return;

            RemoveView(_nativeView);
            _nativeView = newNativeView;
            if (newNativeView != null)
            {
                AddView(_nativeView, this.Width, this.Height);
                this.RequestLayout();
            }
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            var androidHandler = _virtualView?.BuiltView?.ViewHandler as AndroidViewHandler;
            if (_nativeView == null) return;
            _nativeView.Layout(l, t, r, b);
            var rect = new RectangleF(l, t, Math.Abs(l - r), Math.Abs(b - t));
            _virtualView.SetFrameFromNativeView(rect);
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
                CurrentView?.Dispose();
            base.Dispose(disposing);
        }
    }
}
