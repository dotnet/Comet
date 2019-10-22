using System;
using System.Collections.Generic;
using System.Text;

namespace Comet.Styles
{
    public class Style
    {
        
        public ButtonStyle Button { get; set; } = new ButtonStyle();

        public TextStyle Label { get; set; } = new TextStyle
        {
            StyleId = nameof(Label),
        };

        public TextStyle H1 { get; set; } = new TextStyle
        {
            StyleId = nameof(H1),
            Font = new FontAttributes
            {
                Size = 96,
                Weight = Weight.Light,
            },
        };

        public TextStyle H2 { get; set; } = new TextStyle
        {
            StyleId = nameof(H2),
            Font = new FontAttributes
            {
                Size = 60,
                Weight = Weight.Light,
            },
        };

        public TextStyle H3 { get; set; } = new TextStyle
        {
            StyleId = nameof(H3),
            Font = new FontAttributes
            {
                Size = 48,
                Weight = Weight.Regular,
            },
        };

        public TextStyle H4 { get; set; } = new TextStyle
        {
            StyleId = nameof(H4),
            Font = new FontAttributes
            {
                Size = 34,
                Weight = Weight.Regular,
            },
        };

        public TextStyle H5 { get; set; } = new TextStyle
        {
            StyleId = nameof(H5),
            Font = new FontAttributes
            {
                Size = 24,
                Weight = Weight.Regular,
            },
        };

        public TextStyle H6 { get; set; } = new TextStyle
        {
            StyleId = nameof(H6),
            Font = new FontAttributes
            {
                Size = 20,
                Weight = Weight.Medium,
            },
        };

        public TextStyle Subtitle1 { get; set; } = new TextStyle
        {
            StyleId = nameof(Subtitle1),
            Font = new FontAttributes
            {
                Size = 16,
                Weight = Weight.Regular,
            },
        };

        public TextStyle Subtitle2 { get; set; } = new TextStyle
        {
            StyleId = nameof(Subtitle2),
            Font = new FontAttributes
            {
                Size = 13,
                Weight = Weight.Medium,
            },
        };

        public TextStyle Body1 { get; set; } = new TextStyle
        {
            StyleId = nameof(Body1),
            Font = new FontAttributes
            {
                Size = 16,
                Weight = Weight.Regular,
            },
        };

        public TextStyle Body2 { get; set; } = new TextStyle
        {
            StyleId = nameof(Body2),
            Font = new FontAttributes
            {
                Size = 14,
                Weight = Weight.Medium,
            },
        };

        public TextStyle Caption { get; set; } = new TextStyle
        {
            StyleId = nameof(Caption),
            Font = new FontAttributes
            {
                Size = 12,
                Weight = Weight.Regular,
            },
        };

        public TextStyle Overline { get; set; } = new TextStyle
        {
            StyleId = nameof(Overline),
            Font = new FontAttributes
            {
                Size = 10,
                Weight = Weight.Regular,
            },
        };

        public virtual void Apply()
        {
            ApplyButton();
            ApplyTextStyle(Label);
            ApplyTextStyle(H1);
            ApplyTextStyle(H2);
            ApplyTextStyle(H3);
            ApplyTextStyle(H4);
            ApplyTextStyle(H5);
            ApplyTextStyle(H6);
            ApplyTextStyle(Subtitle1);
            ApplyTextStyle(Subtitle2);
            ApplyTextStyle(Body1);
            ApplyTextStyle(Body2);
            ApplyTextStyle(Caption);
            ApplyTextStyle(Overline);

        }


        public virtual void ApplyTextStyle(TextStyle textStyle)
        {
            View.SetGlobalEnvironment(textStyle.FormatedId(EnvironmentKeys.Colors.Color), textStyle.Color);
            View.SetGlobalEnvironment(textStyle.FormatedId(EnvironmentKeys.Fonts.Size), textStyle?.Font?.Size);
            View.SetGlobalEnvironment(textStyle.FormatedId(EnvironmentKeys.Fonts.Family), textStyle?.Font?.Family);
            View.SetGlobalEnvironment(textStyle.FormatedId(EnvironmentKeys.Fonts.Italic), textStyle?.Font?.Italic);
            View.SetGlobalEnvironment(textStyle.FormatedId(EnvironmentKeys.Fonts.Weight), textStyle?.Font?.Weight);
        }

        protected virtual void ApplyButton()
        {
            View.SetGlobalEnvironment(typeof(Button), EnvironmentKeys.Colors.Color, Button?.TextColor);
            //Set the BorderStyle

            View.SetGlobalEnvironment(typeof(Button), EnvironmentKeys.View.ClipShape, Button?.Border);
            View.SetGlobalEnvironment(typeof(Button), EnvironmentKeys.View.Overlay, Button?.Border);
            View.SetGlobalEnvironment(typeof(Button), EnvironmentKeys.Colors.BackgroundColor, Button?.BackgroundColor);

            View.SetGlobalEnvironment(typeof(Button), EnvironmentKeys.View.Shadow, Button?.Shadow);
        }

    }
}
