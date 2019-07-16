using Android.Content;
using Android.Widget;

// ReSharper disable MemberCanBePrivate.Global

namespace HotUI.Android.Handlers
{
    public class TextHandler : AbstractHandler<Text, TextView>
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

        protected override void DisposeView(TextView nativeView)
        {
            
        }

        public static void MapValueProperty(IViewHandler viewHandler, Text virtualView)
        {
            var nativeView = (TextView)viewHandler.NativeView;
            nativeView.Text = virtualView.Value;
        }

        public static void MapFontProperty(IViewHandler viewHandler, Text virtualView)
        {
        }

        public static void MapColorProperty(IViewHandler viewHandler, Text virtualView)
        {
        }
    }
}