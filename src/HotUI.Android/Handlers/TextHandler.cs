using Android.Content;
using Android.Widget;
using AView = Android.Views.View;

namespace HotUI.Android
{
    public class TextHandler : AbstractHandler<Text,TextView>
    {
        public static readonly PropertyMapper<Text> Mapper = new PropertyMapper<Text>(ViewHandler.Mapper)
        {
            [nameof(HotUI.Text.Value)] = MapValueProperty,
            [EnvironmentKeys.Fonts.Font] = MapFontProperty,
            [EnvironmentKeys.Colors.Color] = MapColorProperty,
        };

        public TextHandler() : base(Mapper)
        {
            
        }
        
        protected override TextView CreateView(Context context)
        {
            return new TextView(context);
        }

        public static bool MapValueProperty(IViewHandler viewHandler, Text virtualView)
        {
            var nativeView = (TextView) viewHandler.NativeView;
            nativeView.Text = virtualView.Value;
            return true;
        }

        public static bool MapFontProperty (IViewHandler viewHandler, Text virtualView)
        {
            return true;
        }

        public static bool MapColorProperty(IViewHandler viewHandler, Text virtualView)
        {
            return true;
        }
    }
}