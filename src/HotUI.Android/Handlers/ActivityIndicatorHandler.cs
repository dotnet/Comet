using System;
using Android.Content;
using AndroidApp = Android.Widget;

namespace HotUI.Android.Handlers
{
    public class ActivityIndicatorHandler : AbstractControlHandler<ActivityIndicator, AndroidApp.ProgressBar>
    {
        public static readonly PropertyMapper<ActivityIndicator> Mapper = new PropertyMapper<ActivityIndicator>(ViewHandler.Mapper)
        {
            [EnvironmentKeys.Colors.Color] = MapColorProperty,
        };

        private static Color DefaultColor;

        public ActivityIndicatorHandler() : base(Mapper)
        {
        }

        protected override AndroidApp.ProgressBar CreateView(Context context)
        {
            var activityIndicator = new AndroidApp.ProgressBar(context);
            activityIndicator.Indeterminate = true;

            if(DefaultColor == null)
            {
                // get default color
                DefaultColor = activityIndicator.ProgressTintList.DefaultColor.ToColor();
            }

            return activityIndicator;
        }

        protected override void DisposeView(AndroidApp.ProgressBar nativeView)
        {
            
        }

        public static void MapColorProperty(IViewHandler viewHandler, ActivityIndicator virtualView)
        {
            var nativeView = (AndroidApp.ProgressBar)viewHandler.NativeView;
            var color = virtualView.GetColor(DefaultColor);

            //TODO: check how to set color
            //nativeView.progresstint = color.
        }
    }
}
