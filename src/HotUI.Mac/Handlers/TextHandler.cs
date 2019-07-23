using AppKit;
using HotUI.Mac.Extensions;

namespace HotUI.Mac.Handlers
{
    public class TextHandler : AbstractControlHandler<Text,NSTextField>
    {
        public static readonly PropertyMapper<Text> Mapper = new PropertyMapper<Text>(ViewHandler.Mapper)
        {
            [nameof(Text.TextValue)] = MapValueProperty,
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

        protected override void DisposeView(NSTextField nativeView)
        {
            
        }

        public static void MapValueProperty(IViewHandler viewHandler, Text virtualView)
        {
            var nativeView = (NSTextField) viewHandler.NativeView;
            nativeView.StringValue = virtualView.TextValue;
            virtualView.InvalidateMeasurement();
        }

        public static void MapFontProperty(IViewHandler viewHandler, Text virtualView)
        {
            var nativeView = (NSTextField) viewHandler.NativeView;
            var font = virtualView.GetFont(DefaultFont);
            nativeView.Font = font.ToNSFont();
            virtualView.InvalidateMeasurement();
        }

        public static void MapColorProperty(IViewHandler viewHandler, Text virtualView)
        {
            var nativeView = (NSTextField) viewHandler.NativeView;
            var color = virtualView.GetColor(DefaultColor);
            var nativeColor = nativeView.TextColor.ToColor();
            if (!color.Equals(nativeColor))
                nativeView.TextColor = color.ToNSColor();
        }
    }
}