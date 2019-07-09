using UIKit;
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace HotUI.iOS
{
    public class TextHandler : AbstractHandler<Text, UILabel>
    {
        public static readonly PropertyMapper<Text> Mapper = new PropertyMapper<Text>(ViewHandler.Mapper)
        {
            [nameof(HotUI.Text.Value)] = MapValueProperty,
            [EnvironmentKeys.Fonts.Font] = MapFontProperty,
            [EnvironmentKeys.Colors.Color] = MapColorProperty,
        };

        private static Font DefaultFont;
        private static Color DefaultColor;
        
		public TextHandler () : base(Mapper)
		{

        }

        protected override UILabel CreateView()
        {
            var label = new UILabel();
            
            // todo: answer the question of whether or not these should be default or not.
            if (DefaultColor == null)
            {
                DefaultFont = label.Font.ToFont();
                DefaultColor = label.TextColor.ToColor();
            }

            return label;
        }
        
        public static bool MapValueProperty(IViewHandler viewHandler, Text virtualView)
        {
            var nativeView = (UILabel) viewHandler.NativeView;
            nativeView.Text = virtualView.Value;
            nativeView.SizeToFit();
            return true;
        }

		public static bool MapFontProperty (IViewHandler viewHandler, Text virtualView)
		{
            var nativeView = (UILabel) viewHandler.NativeView;
            var font = virtualView.GetFont(DefaultFont);
			nativeView.Font = font.ToUIFont();
			nativeView.SizeToFit ();
			return true;
		}

        public static bool MapColorProperty(IViewHandler viewHandler, Text virtualView)
        {
            var nativeView = (UILabel) viewHandler.NativeView;
            var color = virtualView.GetColor(DefaultColor);
            var nativeColor = nativeView.TextColor.ToColor();
            if (!color.Equals(nativeColor))
                nativeView.TextColor = color.ToUIColor();

            return true;
        }
    }
}