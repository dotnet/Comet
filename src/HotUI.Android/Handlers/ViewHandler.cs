using System;
using Android.Content;
using AView = Android.Views.View;

namespace HotUI.Android.Handlers
{
    public class ViewHandler : AbstractHandler<View, AView>
    {
        public static readonly PropertyMapper<View> Mapper = new PropertyMapper<View>()
        {
            [nameof(EnvironmentKeys.Colors.BackgroundColor)] = MapBackgroundColorProperty,
            [nameof(EnvironmentKeys.View.Shadow)] = MapShadowProperty,
            [nameof(EnvironmentKeys.View.ClipShape)] = MapClipShapeProperty
        };

        public ViewHandler() : base(Mapper)
        {
        }
        
        protected override AView CreateView(Context context)
        {
            return VirtualView.ToView();
        }

        public static void MapBackgroundColorProperty(IViewHandler handler, View virtualView)
        {
            var nativeView = (AView) handler.NativeView;
            var color = virtualView.GetBackgroundColor();
            if (color != null)
                nativeView.SetBackgroundColor(color.ToColor());
        }

        public static void MapShadowProperty(IViewHandler handler, View virtualView)
        {
            Console.WriteLine("Shadows not yet supported on Android");
        }

        public static void MapClipShapeProperty(IViewHandler handler, View virtualView)
        {
            Console.WriteLine("ClipShape not yet supported on Android");
        }
    }
}