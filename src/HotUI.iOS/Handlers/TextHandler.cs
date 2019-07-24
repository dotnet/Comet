using UIKit;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace HotUI.iOS.Handlers
{
    public class TextHandler : AbstractControlHandler<Text, UILabel>
    {
        public static readonly PropertyMapper<Text> Mapper = new PropertyMapper<Text>(ViewHandler.Mapper)
        {
            [nameof(HotUI.Text.Value)] = MapValueProperty,
            [EnvironmentKeys.Fonts.Family] = MapFontProperty,
            [EnvironmentKeys.Fonts.Italic] = MapFontProperty,
            [EnvironmentKeys.Fonts.Size] = MapFontProperty,
            [EnvironmentKeys.Fonts.Weight] = MapFontProperty,
            [EnvironmentKeys.Colors.Color] = MapColorProperty,
        };

        private static FontAttributes DefaultFont;
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

        protected override void DisposeView(UILabel nativeView)
        {
            
        }

        public static void MapValueProperty(IViewHandler viewHandler, Text virtualView)
        {
            var nativeView = (UILabel) viewHandler.NativeView;
            nativeView.Text = virtualView.Value;
            virtualView.InvalidateMeasurement();
        }

		public static void MapFontProperty (IViewHandler viewHandler, Text virtualView)
		{
            var nativeView = (UILabel) viewHandler.NativeView;
            var font = virtualView.GetFont(DefaultFont);
			nativeView.Font = font.ToUIFont();
            virtualView.InvalidateMeasurement();
        }

        public static void MapColorProperty(IViewHandler viewHandler, Text virtualView)
        {
            var nativeView = (UILabel) viewHandler.NativeView;
            var color = virtualView.GetColor(DefaultColor);
            nativeView.TextColor = color.ToUIColor();
        }
    }
}