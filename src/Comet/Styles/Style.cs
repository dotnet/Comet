using System;
using System.Collections.Generic;
using System.Text;

namespace Comet.Styles
{
    public class Style
    {
        public ButtonStyle Button { get; set; } = new ButtonStyle();

        public NavbarStyle Navbar { get; set; } = new NavbarStyle();

        public TextStyle Label { get; set; } = new TextStyle
        {
            StyleId = nameof(Label)
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

        public virtual void Apply(ContextualObject view = null)
        {
            ApplyButton(view);
            ApplyNavbarStyles(view);
            ApplyTextStyle(view, Label);
            ApplyTextStyle(view, H1);
            ApplyTextStyle(view, H2);
            ApplyTextStyle(view, H3);
            ApplyTextStyle(view, H4);
            ApplyTextStyle(view, H5);
            ApplyTextStyle(view, H6);
            ApplyTextStyle(view, Subtitle1);
            ApplyTextStyle(view, Subtitle2);
            ApplyTextStyle(view, Body1);
            ApplyTextStyle(view, Body2);
            ApplyTextStyle(view, Caption);
            ApplyTextStyle(view, Overline);
        }


        protected virtual void ApplyTextStyle(ContextualObject view, TextStyle textStyle)
        {
            SetEnvironement(view, textStyle.StyleId, EnvironmentKeys.Colors.Color, textStyle.Color);
            SetEnvironement(view, textStyle.StyleId, EnvironmentKeys.Fonts.Size, textStyle?.Font?.Size);
            SetEnvironement(view, textStyle.StyleId, EnvironmentKeys.Fonts.Family, textStyle?.Font?.Family);
            SetEnvironement(view, textStyle.StyleId, EnvironmentKeys.Fonts.Italic, textStyle?.Font?.Italic);
            SetEnvironement(view, textStyle.StyleId, EnvironmentKeys.Fonts.Weight, textStyle?.Font?.Weight);
        }

        protected virtual void ApplyButton(ContextualObject view)
        {
            SetEnvironement(view, typeof(Button), EnvironmentKeys.Colors.Color, Button?.TextColor);
            //Set the BorderStyle

            SetEnvironement(view, typeof(Button), EnvironmentKeys.View.ClipShape, Button?.Border);
            SetEnvironement(view, typeof(Button), EnvironmentKeys.View.Overlay, Button?.Border);
            SetEnvironement(view, typeof(Button), EnvironmentKeys.Colors.BackgroundColor, Button?.BackgroundColor);

            SetEnvironement(view, typeof(Button), EnvironmentKeys.View.Shadow, Button?.Shadow);
        }


        protected virtual void ApplyNavbarStyles(ContextualObject view)
        {
            SetEnvironement(view, "", EnvironmentKeys.Navigation.BackgroundColor, Navbar?.BackgroundColor);
            SetEnvironement(view, "", EnvironmentKeys.Navigation.TextColor, Navbar?.TextColor);
        }

        protected void SetEnvironement(ContextualObject view, Type type, string key, object value)
        {
            if (view != null)
                view.SetEnvironment(type, key, value);
            else
                View.SetGlobalEnvironment(type, key, value);
        }

        protected void SetEnvironement(ContextualObject view, string styleId, string key, object value)
        {
            if (view != null)
                view.SetEnvironment(styleId, key, value);
            else
                View.SetGlobalEnvironment(styleId, key, value);
        }
    }
}
