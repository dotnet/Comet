using AppKit;
using HotUI.Mac.Extensions;

namespace HotUI.Mac.Handlers
{
    public class TextHandler : AbstractHandler<Text,NSTextField>
    {
        public static readonly PropertyMapper<Text> Mapper = new PropertyMapper<Text>(ViewHandler.Mapper)
        {
            [nameof(Text.Value)] = MapValueProperty,
            [EnvironmentKeys.Fonts.Font] = MapFontProperty,
            [EnvironmentKeys.Colors.Color] = MapColorProperty,
        };
        
        private static Font DefaultFont;
        private static Color DefaultColor;

        public TextHandler() : base(Mapper)
        {
        }
        
        protected override NSTextField CreateView()
        {
            var textField = new NSTextField();
            
            if (DefaultColor == null)
            {
                DefaultFont = textField.Font.ToFont();
                DefaultColor = textField.TextColor.ToColor();
            }

            textField.Editable = false;
            textField.Bezeled = false;
            textField.DrawsBackground = false;
            textField.Selectable = false;

            return textField;
        }
        
        public static bool MapValueProperty(IViewHandler viewHandler, Text virtualView)
        {
            var nativeView = (NSTextField) viewHandler.NativeView;
            nativeView.StringValue = virtualView.Value;
            nativeView.SizeToFit();
            return true;
        }

        public static bool MapFontProperty(IViewHandler viewHandler, Text virtualView)
        {
            var nativeView = (NSTextField) viewHandler.NativeView;
            var font = virtualView.GetFont(DefaultFont);
            nativeView.Font = font.ToUIFont();
            nativeView.SizeToFit();
            return true;
        }

        public static bool MapColorProperty(IViewHandler viewHandler, Text virtualView)
        {
            var nativeView = (NSTextField) viewHandler.NativeView;
            var color = virtualView.GetColor(DefaultColor);
            var nativeColor = nativeView.TextColor.ToColor();
            if (!color.Equals(nativeColor))
                nativeView.TextColor = color.ToNSColor();

            return true;
        }
    }
}