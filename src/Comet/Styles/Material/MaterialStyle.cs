using System;
using System.Collections.Generic;

using System.Text;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace Comet.Styles.Material
{
	public class MaterialStyle : Style
	{
		public const string OutlinedButtonStyleId = "MaterialOutlinedButtonStyleId";
		public const string ContainedButtonStyleId = "MaterialContainedButton";
		public const string TextButtonStyleId = "MaterialTextButton";
		public MaterialStyle()
		{

		}
		public ColorPalette PrimaryColorPalette { get; private set; } = ColorPalette.Blue;
		public ColorPalette SecondaryColorPalette { get; private set; } = ColorPalette.Blue;

		public ButtonStyle OutlinedButton { get; set; }
		public ButtonStyle ContainedButton { get; set; }
		public ButtonStyle TextButton { get; set; }

		public MaterialStyle(ColorPalette colorPalette)
		{
			PrimaryColorPalette = colorPalette;
			SecondaryColorPalette = colorPalette;

			Button = OutlinedButton = new ButtonStyle
			{
				TextColor = colorPalette.P900,
				Border = new RoundedRectangle(4f).Stroke(Colors.Grey, 1f, true),
				BackgroundColor = new StyleAwareValue<ControlState, Color>
				{
					[ControlState.Default] = colorPalette.PD900,
					[ControlState.Hovered] = Colors.Grey.WithAlpha(.6f),
					[ControlState.Pressed] = Colors.Grey.WithAlpha(.4f),
				},
				Padding = new Thickness(16, 0, 16, 0),
				Shadow = null,
			};

			ContainedButton = new ButtonStyle
			{
				TextColor = colorPalette.PD900,
				Border = new RoundedRectangle(4f).Stroke(Colors.Grey, 1f, true),
				BackgroundColor = new StyleAwareValue<ControlState, Color>
				{
					[ControlState.Default] = colorPalette.P900,
					[ControlState.Hovered] = colorPalette.P800,
					[ControlState.Pressed] = colorPalette.P700,
				},
				Shadow = new Graphics.Shadow().WithColor(Colors.Grey).WithRadius(1).WithOffset(new Point(1, 1)),
				Padding = new Thickness(16, 0, 16, 0),
			};

			TextButton = new ButtonStyle
			{
				TextColor = colorPalette.P900,
				Padding = new Thickness(16, 0, 16, 0),
				BackgroundColor = new StyleAwareValue<ControlState, Color>
				{
					[ControlState.Default] = Colors.Transparent,
					[ControlState.Hovered] = Colors.Grey.WithAlpha(.6f),
					[ControlState.Pressed] = Colors.Grey.WithAlpha(.4f),
				},
				Shadow = null,
				Border = null,
			};

			Navbar = new NavbarStyle
			{
				BackgroundColor = colorPalette.P500,
				TextColor = colorPalette.PD500
			};

			Slider = new SliderStyle
			{
				ThumbColor = colorPalette.P500,
				ProgressColor = colorPalette.P500,
				TrackColor = colorPalette.P100,
			};

			ProgressBar = new ProgressBarStyle
			{
				ProgressColor = colorPalette.P500,
			};
		}

		protected override void ApplyButton(ContextualObject view)
		{
			base.ApplyButton(view);
			ApplyButton(view, OutlinedButtonStyleId, OutlinedButton);
			ApplyButton(view, ContainedButtonStyleId, ContainedButton);
			ApplyButton(view, TextButtonStyleId, TextButton);

		}

		protected virtual void ApplyButton(ContextualObject view, string styleId, ButtonStyle style)
		{
			SetEnvironment(view, styleId, EnvironmentKeys.Colors.Color, style?.TextColor);

			//Set the BorderStyle

			SetEnvironment(view, styleId, EnvironmentKeys.View.Border, style?.Border);
			SetEnvironment(view, styleId, EnvironmentKeys.Colors.Background, style?.BackgroundColor);

			SetEnvironment(view, styleId, EnvironmentKeys.View.Shadow, style?.Shadow);
		}
	}
}
