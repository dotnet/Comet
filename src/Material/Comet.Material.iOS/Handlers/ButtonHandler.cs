using System;
using Comet.iOS;
using Comet.iOS.Handlers;
using MaterialComponents;
using UIKit;
using MButton = MaterialComponents.Button;
namespace Comet.Material.iOS
{
    public class ButtonHandler : AbstractControlHandler<Button, MButton>
    {
        public static readonly PropertyMapper<Button> Mapper = new PropertyMapper<Button>(ViewHandler.Mapper)
        {
            [nameof(Button.Text)] = MapTextProperty,
            [nameof(EnvironmentKeys.View.Overlay)] = MapOverlayProperty,
            //[EnvironmentKeys.Colors.Color] = MapColorProperty,
        };

        public ButtonHandler() : base(Mapper)
        {

        }

        private static FontAttributes DefaultFont;
        private static Color DefaultColor;
        private static Color DefaultSelectedColor;
       
        ButtonScheme buttonScheme;
        protected override MButton CreateView()
        {
            var button = new MButton();

            if (DefaultColor == null)
            {
                DefaultFont = button.Font.ToFont();
                DefaultColor = button.TitleColor(UIControlState.Normal).ToColor();
                DefaultSelectedColor = button.TitleColor(UIControlState.Highlighted).ToColor();
            }
            var semanticColorScheme = new SemanticColorScheme(ColorSchemeDefaults.Material201804)
            {

            };
            var typographyScheme = new TypographyScheme();
            buttonScheme = new ButtonScheme
            {
                ColorScheme = semanticColorScheme,
                TypographyScheme = typographyScheme,
                 ShapeScheme = new ShapeScheme(),
            };

            button.TouchUpInside += HandleTouchUpInside;
            //button.SetTitleColor(UIColor.Blue, UIControlState.Normal);
            ContainedButtonThemer.ApplyScheme(buttonScheme, button);
            /*Layer.BorderColor = UIColor.Blue.CGColor;
            Layer.BorderWidth = .5f;
            Layer.CornerRadius = 3f;*/

            return button;
        }

        protected override void DisposeView(MButton button)
        {
            button.TouchUpInside -= HandleTouchUpInside;
        }

        private void HandleTouchUpInside(object sender, EventArgs e)
        {
            VirtualView?.OnClick?.Invoke();
        }

        public static void MapTextProperty(IViewHandler viewHandler, Button virtualView)
        {
            var nativeView = (UIButton)viewHandler.NativeView;
            nativeView.SetTitle(virtualView.Text?.CurrentValue, UIControlState.Normal);
            virtualView.InvalidateMeasurement();
        }

        public static void MapColorProperty(IViewHandler viewHandler, Button virtualView)
        {
            var nativeView = (UIButton)viewHandler.NativeView;
            nativeView.SetTitleColor(virtualView.GetColor(DefaultColor).ToUIColor(), UIControlState.Normal);
        }


        public static void MapOverlayProperty(IViewHandler handler, View virtualView)
        {

        }
    }
}
