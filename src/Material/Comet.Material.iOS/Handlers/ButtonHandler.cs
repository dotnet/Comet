using System;
using Comet.iOS;
using Comet.iOS.Handlers;
using MaterialComponents;
using UIKit;
using MButton = MaterialComponents.Button;
namespace Comet.Material.iOS
{
    public class ButtonHandler : MaterialViewHandler<Button, MButton>
    {
        public static readonly PropertyMapper<Button> Mapper = new PropertyMapper<Button>(MaterialViewHandler.Mapper)
        {
            [nameof(Button.Text)] = MapTextProperty,
            [EnvironmentKeys.Colors.Color] = MapColorProperty,
            [nameof(EnvironmentKeys.Colors.BackgroundColor)] = MapBackgroundColorProperty,
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
          

            button.TouchUpInside += HandleTouchUpInside;

            RecreateScheme();
            ApplyScheme();

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
            var materialView = viewHandler as IMaterialView;
            var color = virtualView.GetColor(null);
           materialView.ColorScheme.SecondaryColor = color?.ToUIColor();

            materialView.ApplyScheme();
        }

        public static void MapBackgroundColorProperty(IViewHandler handler, Button virtualView)
        {
            var materialView = handler as IMaterialView;
            var color = virtualView.GetBackgroundColor();
            if (color != null)
                materialView.ColorScheme.PrimaryColor = color.ToUIColor();

            materialView.ApplyScheme();
        }

        public override void ApplyScheme()
        {
            if (TypedNativeView == null)
                return;
            ContainedButtonThemer.ApplyScheme(buttonScheme, TypedNativeView);
        }

        public override void RecreateScheme()
        {
            buttonScheme = new ButtonScheme
            {
                ColorScheme = this.ColorScheme,
                TypographyScheme = this.TypographyScheme,
                ShapeScheme = this.ShapeScheme,
            };
            ApplyScheme();
        }
    }
}
