using Android.Content;
using Android.Graphics;
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
            [nameof(EnvironmentKeys.Text.Alignment)] = MapTextAlignmentProperty,
            //TODO: this may cause a lot of font setting
            [EnvironmentKeys.Fonts.Family] = MapFontProperty,
            [EnvironmentKeys.Fonts.Italic] = MapFontProperty,
            [EnvironmentKeys.Fonts.Size] = MapFontProperty,
            [EnvironmentKeys.Fonts.Weight] = MapFontProperty,
            [EnvironmentKeys.Colors.Color] = MapColorProperty,
        };

        private static FontAttributes DefaultFont;
        static Color DefaultColor;

        public TextHandler() : base(Mapper)
        {
        }

        protected override TextView CreateView(Context context)
        {
            var textView = new TextView(context);
            if(DefaultColor == null)
            {
                DefaultFont = new FontAttributes
                {
                    Italic = textView.Typeface.IsItalic,
                    Size = 16,
                    Weight = Weight.Regular
                };
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
            nativeView.Text = virtualView.Value?.CurrentValue ?? string.Empty;
        }

        public static void MapTextAlignmentProperty(IViewHandler viewHandler, Text virtualView)
        {
            var nativeView = (TextView) viewHandler.NativeView;
            var textAlignment = virtualView.GetTextAlignment();
            nativeView.TextAlignment = textAlignment.ToAndroidTextAlignment();
            virtualView.InvalidateMeasurement();
        }
        
        public static void MapFontProperty(IViewHandler viewHandler, Text virtualView)
        {
            var nativeView = (TextView)viewHandler.NativeView;
            var font = virtualView.GetFont(DefaultFont);

            // note: default values (fonts) should be discussed at a more abstract level first
            if (font == DefaultFont)
                return;

            TypefaceStyle typefaceStyle = TypefaceStyle.Normal;
            switch (font.Weight)
            {
                case Weight.Bold:
                    typefaceStyle = font.Italic ? TypefaceStyle.BoldItalic : TypefaceStyle.Bold;
                    break;
                case Weight.Regular:
                    typefaceStyle = font.Italic ? TypefaceStyle.Italic : TypefaceStyle.Normal;
                    break;
            }
            nativeView.SetTypeface(nativeView.Typeface, typefaceStyle);
            nativeView.TextSize = font.Size;
            virtualView.InvalidateMeasurement();
        }

        public static void MapColorProperty(IViewHandler viewHandler, Text virtualView)
        {
            var textView = viewHandler.NativeView as TextView;
            var color = virtualView.GetColor(DefaultColor).ToColor();
            textView.SetTextColor(color);
        }
    }
}
