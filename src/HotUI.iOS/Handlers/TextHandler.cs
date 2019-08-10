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
            [EnvironmentKeys.LineBreakMode.Mode] = MapLineBreakModeProperty,
        };

        private static FontAttributes DefaultFont;
        private static Color DefaultColor;
        private static LineBreakMode DefaultLineBreakMode;

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

            if(DefaultLineBreakMode == null)
            {
                DefaultLineBreakMode = LineBreakMode.NoWrap;
            }

            return label;
        }

        protected override void DisposeView(UILabel nativeView)
        {
            
        }

        public static void MapValueProperty(IViewHandler viewHandler, Text virtualView)
        {
            var nativeView = (UILabel) viewHandler.NativeView;
            nativeView.Text = virtualView.Value.Get();
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

        public static void MapLineBreakModeProperty(IViewHandler viewHandler, Text virtualView)
        {
            var nativeView = (UILabel)viewHandler.NativeView;
            var mode = virtualView.GetLineBreakMode(DefaultLineBreakMode);
            nativeView.LineBreakMode = mode.ToUILineBreakMode();
            if(mode == LineBreakMode.WordWrap || mode == LineBreakMode.CharacterWrap)
                nativeView.Lines = 0;
            virtualView.InvalidateMeasurement();
        }
    }
}