using System;
using UIKit;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Comet.iOS.Handlers
{
	public class ButtonHandler : AbstractControlHandler<Button, UIButton>
	{
		public static readonly PropertyMapper<Button> Mapper = new PropertyMapper<Button>(ViewHandler.Mapper)
		{
			[nameof(Button.Text)] = MapTextProperty,
			[EnvironmentKeys.Colors.Color] = MapColorProperty,
			[EnvironmentKeys.Fonts.Family] = MapFontProperty,
			[EnvironmentKeys.Fonts.Italic] = MapFontProperty,
			[EnvironmentKeys.Fonts.Size] = MapFontProperty,
			[EnvironmentKeys.Fonts.Weight] = MapFontProperty,
			[EnvironmentKeys.Colors.Color] = MapColorProperty,
		};

		public ButtonHandler() : base(Mapper)
		{

		}

		private static FontAttributes DefaultFont;
		private static Color DefaultColor;
		private static Color DefaultSelectedColor;
		protected override UIButton CreateView()
		{
			var button = new UIButton(UIButtonType.System);

			if (DefaultColor == null)
			{
				DefaultFont = button.Font.ToFont();
				DefaultColor = button.TitleColor(UIControlState.Normal).ToColor();
				DefaultSelectedColor = button.TitleColor(UIControlState.Highlighted).ToColor();
			}

			button.TouchUpInside += HandleTouchUpInside;
			button.SetTitleColor(UIColor.Blue, UIControlState.Normal);
			/*Layer.BorderColor = UIColor.Blue.CGColor;
            Layer.BorderWidth = .5f;
            Layer.CornerRadius = 3f;*/

			return button;
		}

		protected override void DisposeView(UIButton button)
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

		public static void MapFontProperty(IViewHandler viewHandler, Button virtualView)
		{
			var nativeView = (UIButton)viewHandler.NativeView;
			var font = virtualView.GetFont(DefaultFont);
			nativeView.Font = font.ToUIFont();
			virtualView.InvalidateMeasurement();
		}
	}
}
