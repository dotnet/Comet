using System;
using UIKit;

namespace Comet.iOS.Handlers
{
    public class ProgressBarHandler : AbstractControlHandler<ProgressBar, UIProgressView>
    {
        public static readonly PropertyMapper<ProgressBar> Mapper = new PropertyMapper<ProgressBar>(ViewHandler.Mapper)
        {
            [nameof(ProgressBar.Value)] = MapValueProperty,
            [nameof(ProgressBar.IsIndeterminate)] = MapIsIndeterminateProperty,
        };

        public ProgressBarHandler() : base(Mapper)
        {
        }

        protected override UIProgressView CreateView()
        {
            var progressView = new UIProgressView();

            return progressView;
        }


        public static void MapValueProperty(IViewHandler viewHandler, ProgressBar virtualView)
        {
           var nativeView = (UIProgressView)viewHandler.NativeView;

           nativeView.Progress = (float)virtualView.Value.CurrentValue * 0.01f;
        }

        public static void MapIsIndeterminateProperty(IViewHandler viewHandler, ProgressBar virtualView)
        {
            var nativeView = (UIProgressView)viewHandler.NativeView;
            // TODO Need to use an UIActivityIndicatorView
        }

        protected override void DisposeView(UIProgressView nativeView)
        {
            throw new NotImplementedException();
        }
    }
}
