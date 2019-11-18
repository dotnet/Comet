using System;
using System.Collections.Generic;
using System.Text;

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
                Border = new RoundedRectangle(4f).Stroke(Color.Grey, 1f, true),
                BackgroundColor = colorPalette.PD900,
                Padding = new Thickness(16, 0, 16, 0),
                Shadow = null,
            };

            ContainedButton = new ButtonStyle
            {
                TextColor = colorPalette.PD900,
                Border = new RoundedRectangle(4f).Stroke(Color.Grey, 1f, true),
                BackgroundColor = colorPalette.P900,
                Shadow = new Graphics.Shadow().WithColor(Color.Grey).WithRadius(1).WithOffset(new SizeF(1, 1)),
                Padding = new Thickness(16, 0, 16, 0),
            };

            TextButton = new ButtonStyle
            {
                TextColor = colorPalette.P900,
                Padding = new Thickness(16, 0, 16, 0),
                BackgroundColor = Color.Transparent,
                Shadow = null,
                Border = null,
            };

            Navbar = new NavbarStyle
            {
                BackgroundColor = colorPalette.P500,
                TextColor = colorPalette.PD500
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
            SetEnvironement(view, styleId, EnvironmentKeys.Colors.Color, style?.TextColor);

            //Set the BorderStyle

            SetEnvironement(view, styleId, EnvironmentKeys.View.ClipShape, style?.Border);
            SetEnvironement(view, styleId, EnvironmentKeys.View.Overlay, style?.Border);
            SetEnvironement(view, styleId, EnvironmentKeys.Colors.BackgroundColor, style?.BackgroundColor);

            SetEnvironement(view, styleId, EnvironmentKeys.View.Shadow, style?.Shadow);
        }
    }
}
