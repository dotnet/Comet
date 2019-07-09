using AppKit;
using HotUI.Mac.Extensions;

namespace HotUI.Mac.Handlers
{
    public class TextHandler : NSTextField, INSView
    {
        public static readonly PropertyMapper<Text> Mapper = new PropertyMapper<Text>(ViewHandler.Mapper)
        {
            [nameof(Text.Value)] = MapValueProperty,
            [EnvironmentKeys.Fonts.Font] = MapFontProperty,
            [EnvironmentKeys.Colors.Color] = MapColorProperty,
        };
        
        private static Font DefaultFont;
        private static Color DefaultColor;
        private static Color DefaultBackgroundColor;

        public NSView View => this;

        public object NativeView => View;
        public bool HasContainer { get; set; } = false;

        public TextHandler()
        {
            if (DefaultColor == null)
            {
                DefaultFont = Font.ToFont();
                DefaultColor = TextColor.ToColor();
                DefaultBackgroundColor = BackgroundColor.ToColor();
            }

            Editable = false;
            Bezeled = false;
            DrawsBackground = false;
            Selectable = false;
        }

        public void Remove(View view)
        {
        }

        private Text _text;
        
        public void SetView(View view)
        {
            _text = view as Text;
            Mapper.UpdateProperties(this, _text);
        }

        public void UpdateValue(string property, object value)
        {
            Mapper.UpdateProperty(this, _text, property);
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