using Android.Content;
using Android.Widget;
using Comet.Android;

// ReSharper disable MemberCanBePrivate.Global

namespace Comet.Android.Handlers
{
    public class TextHandler : AbstractControlHandler<Text, TextView>
    {
        public static readonly PropertyMapper<Text> Mapper = new PropertyMapper<Text>(ViewHandler.Mapper)
        {
            [nameof(Text.Value)] = MapValueProperty,
            //TODO: this may cause a lot of font setting
            [EnvironmentKeys.Fonts.Family] = MapFontProperty,
            [EnvironmentKeys.Fonts.Italic] = MapFontProperty,
            [EnvironmentKeys.Fonts.Size] = MapFontProperty,
            [EnvironmentKeys.Fonts.Weight] = MapFontProperty,
            [EnvironmentKeys.Colors.Color] = MapColorProperty,
        };

        public TextHandler() : base(Mapper)
        {
        }
        static Color DefaultColor;
        protected override TextView CreateView(Context context)
        {
            var textView = new TextView(context);
            if(DefaultColor == null)
            {
                DefaultColor = textView.CurrentTextColor.ToColor();
            }
            return textView;
        }

        protected override void DisposeView(TextView nativeView)
        {
        }

        public static void MapValueProperty(IViewHandler viewHandler, Text virtualView)
        {
            var nativeView = (TextView) viewHandler.NativeView;
            nativeView.Text = virtualView.Value?.Get() ?? string.Empty;
        }

        public static void MapFontProperty(IViewHandler viewHandler, Text virtualView)
        {
        }

        public static void MapColorProperty(IViewHandler viewHandler, Text virtualView)
        {
            var textView = viewHandler.NativeView as TextView;
            var color = virtualView.GetColor(DefaultColor).ToColor();
            textView.SetTextColor(color);

        }
    }
}
