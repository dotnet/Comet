using System;
using Android.Content;
using Android.Widget;
using AProgressBar = Android.Widget.ProgressBar;
using AAttribute = Android.Resource.Attribute;

namespace HotUI.Android.Handlers
{
    public class ProgressBarHandler : AbstractControlHandler<ProgressBar, AProgressBar>
    {
        public static readonly PropertyMapper<ProgressBar> Mapper = new PropertyMapper<ProgressBar>(ViewHandler.Mapper)
        {
            [nameof(ProgressBar.Value)] = MapValueProperty,
            [nameof(ProgressBar.IsIndeterminate)] = MapIsIndeterminateProperty,
        };

        public ProgressBarHandler() : base(Mapper)
        {
        }

        protected override AProgressBar CreateView(Context context)
        {
            var progressView = new AProgressBar(context, null, AAttribute.ProgressBarStyleHorizontal);
            progressView.Max = 100;
            return progressView;
        }

        public static void MapValueProperty(IViewHandler viewHandler, ProgressBar virtualView)
        {
           var nativeView = (AProgressBar)viewHandler.NativeView;
           
           nativeView.Progress = (int)virtualView.Value;
        }

        public static void MapIsIndeterminateProperty(IViewHandler viewHandler, ProgressBar virtualView)
        {
            var nativeView = (AProgressBar)viewHandler.NativeView;
            nativeView.Indeterminate = virtualView.IsIndeterminate;
        }

        protected override void DisposeView(AProgressBar nativeView)
        {
            throw new NotImplementedException();
        }

  
    }
}