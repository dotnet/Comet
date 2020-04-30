using System;
using System.Maui;
using System.Maui.Styles;

namespace System.Maui.Samples
{
	public static class Extensions
	{
		public static T StyleAsCircleButton<T>(this T button) where T : Button
		{
			button.StyleId = MauiStyle.CircleButtonStyleId;
			return button;
		}
	}
	public class MauiStyle : Style
	{
		public const string CircleButtonStyleId = "MauiCircleButton";

		public ButtonStyle CircleButton { get; set; }
		static Color Purple = new Color("#512bd4");
		public MauiStyle()
		{
			Navbar = new NavbarStyle
			{
				BackgroundColor = Purple,
				TextColor = Color.White,
			};
			CircleButton = new ButtonStyle
			{
				BackgroundColor = new StyleAwareValue<ControlState, Color>
				{
					[ControlState.Default] = Purple,
					[ControlState.Pressed] = Purple.WithAlpha(.8f),
				},
				TextColor = Color.White,
				ClipShape = new Circle(),
				Shadow = new System.Maui.Graphics.Shadow().WithOffset(new System.Drawing.SizeF(2,4)).WithRadius(4),
			};
		}
		protected override void ApplyButton(ContextualObject view)
		{
			base.ApplyButton(view);
			ApplyButton(view, CircleButtonStyleId, CircleButton);

		}

		protected virtual void ApplyButton(ContextualObject view, string styleId, ButtonStyle style)
		{
			SetEnvironment(view, styleId, EnvironmentKeys.Colors.Color, style?.TextColor);

			//Set the BorderStyle
			SetEnvironment(view, styleId, EnvironmentKeys.Layout.FrameConstraints,
				(StyleAwareValue<ControlState, FrameConstraints>)new FrameConstraints(94, 66, Alignment.Trailing));
			SetEnvironment(view, styleId, EnvironmentKeys.Fonts.Size, (StyleAwareValue<ControlState, float>)30);
			SetEnvironment(view, styleId, EnvironmentKeys.View.Border, style?.Border);
			SetEnvironment(view, styleId, EnvironmentKeys.View.Overlay, style?.Border);
			SetEnvironment(view, styleId, EnvironmentKeys.View.ClipShape, style?.ClipShape);
			SetEnvironment(view, styleId, EnvironmentKeys.Colors.BackgroundColor, style?.BackgroundColor);
			SetEnvironment(view, styleId, EnvironmentKeys.View.Shadow, style?.Shadow);
		}
	}
}
