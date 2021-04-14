using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui;
using Microsoft.Maui.Primitives;

namespace Comet.Styles
{
	public class Style
	{
		public ButtonStyle Button { get; set; } = new ButtonStyle();

		public NavbarStyle Navbar { get; set; } = new NavbarStyle();

		public SliderStyle Slider { get; set; } = new SliderStyle();

		public ProgressBarStyle ProgressBar { get; set; } = new ProgressBarStyle();

		public TextStyle Label { get; set; } = new TextStyle
		{
			StyleId = nameof(Label)
		};

		public TextStyle H1 { get; set; } = new TextStyle
		{
			StyleId = nameof(H1),
			Font = Font.Default.WithSize(96)
			//{
			//	Size = 96,
			//	Weight = Weight.Light,
			//},
		};

		public TextStyle H2 { get; set; } = new TextStyle
		{
			StyleId = nameof(H2),
			Font = Font.Default.WithSize(60)
			//{
			//	Size = 60,
			//	Weight = Weight.Light,
			//},
		};

		public TextStyle H3 { get; set; } = new TextStyle
		{
			StyleId = nameof(H3),
			Font = Font.Default.WithSize(48)
			//{
			//	Size = 48,
			//	Weight = Weight.Regular,
			//},
		};

		public TextStyle H4 { get; set; } = new TextStyle
		{
			StyleId = nameof(H4),
			Font = Font.Default.WithSize(48)
			//{
			//	Size = 34,
			//	Weight = Weight.Regular,
			//},
		};

		public TextStyle H5 { get; set; } = new TextStyle
		{
			StyleId = nameof(H5),
			Font = Font.Default.WithSize(24)
			//{
			//	Size = 24,
			//	Weight = Weight.Regular,
			//},
		};

		public TextStyle H6 { get; set; } = new TextStyle
		{
			StyleId = nameof(H6),
			Font = Font.Default.WithSize(20)
			//{
			//	Size = 20,
			//	Weight = Weight.Medium,
			//},
		};

		public TextStyle Subtitle1 { get; set; } = new TextStyle
		{
			StyleId = nameof(Subtitle1),
			Font = Font.Default.WithSize(16)
			//{
			//	Size = 16,
			//	Weight = Weight.Regular,
			//},
		};

		public TextStyle Subtitle2 { get; set; } = new TextStyle
		{
			StyleId = nameof(Subtitle2),
			Font = Font.Default.WithSize(13)
			//{
			//	Size = 13,
			//	Weight = Weight.Medium,
			//},
		};

		public TextStyle Body1 { get; set; } = new TextStyle
		{
			StyleId = nameof(Body1),
			Font = Font.Default.WithSize(16)
			//{
			//	Size = 16,
			//	Weight = Weight.Regular,
			//},
		};

		public TextStyle Body2 { get; set; } = new TextStyle
		{
			StyleId = nameof(Body2),
			Font = Font.Default.WithSize(14)
			//{
			//	Size = 14,
			//	Weight = Weight.Medium,
			//},
		};

		public TextStyle Caption { get; set; } = new TextStyle
		{
			StyleId = nameof(Caption),
			Font = Font.Default.WithSize(12)
			//{
			//	Size = 12,
			//	Weight = Weight.Regular,
			//},
		};

		public TextStyle Overline { get; set; } = new TextStyle
		{
			StyleId = nameof(Overline),
			Font = Font.Default.WithSize(10)
			//{
			//	Size = 10,
			//	Weight = Weight.Regular,
			//},
		};

		public virtual void Apply(ContextualObject view = null)
		{
			if (view == null)
				SetDefaultControlSizingForLayouts();
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
			ApplySliderStyle(view);
			ApplyProgresBarStyle(view);
		}


		protected virtual void ApplyTextStyle(ContextualObject view, TextStyle textStyle)
		{
			SetEnvironment(view, textStyle.StyleId, EnvironmentKeys.Colors.Color, textStyle.Color);
			SetEnvironment(view, textStyle.StyleId, EnvironmentKeys.Fonts.Size, textStyle?.Font, (f) => (f is Font font) ? font.FontSize : null);
			SetEnvironment(view, textStyle.StyleId, EnvironmentKeys.Fonts.Family, textStyle?.Font, (f) => (f is Font font) ? font.FontFamily : null);
			SetEnvironment(view, textStyle.StyleId, EnvironmentKeys.Fonts.Attributes, textStyle?.Font, (f) => (f is Font font) ? font.FontAttributes : null);

		}

		protected virtual void ApplyButton(ContextualObject view)
		{
			SetEnvironment(view, typeof(Button), EnvironmentKeys.Colors.Color, Button?.TextColor);
			//Set the BorderStyle
			SetEnvironment(view, typeof(Button), EnvironmentKeys.View.ClipShape, Button?.Border);
			SetEnvironment(view, typeof(Button), EnvironmentKeys.View.Overlay, Button?.Border);
			SetEnvironment(view, typeof(Button), EnvironmentKeys.Colors.BackgroundColor, Button?.BackgroundColor);

			SetEnvironment(view, typeof(Button), EnvironmentKeys.View.Shadow, Button?.Shadow);
		}


		protected virtual void ApplyNavbarStyles(ContextualObject view)
		{
			SetEnvironment(view, "", EnvironmentKeys.Navigation.BackgroundColor, Navbar?.BackgroundColor);
			SetEnvironment(view, "", EnvironmentKeys.Navigation.TextColor, Navbar?.TextColor);
		}


		protected virtual void ApplySliderStyle(ContextualObject view)
		{
			SetEnvironment(view, "", EnvironmentKeys.Slider.TrackColor, Slider?.TrackColor);
			SetEnvironment(view, "", EnvironmentKeys.Slider.ProgressColor, Slider?.ProgressColor);
			SetEnvironment(view, "", EnvironmentKeys.Slider.ThumbColor, Slider?.ThumbColor);
			ApplyViewStyles(view, Slider, typeof(Slider));

		}

		protected virtual void ApplyProgresBarStyle(ContextualObject view)
		{
			SetEnvironment(view, "", EnvironmentKeys.ProgressBar.TrackColor, ProgressBar?.TrackColor);
			SetEnvironment(view, "", EnvironmentKeys.ProgressBar.ProgressColor, ProgressBar?.ProgressColor);
			ApplyViewStyles(view, ProgressBar, typeof(ProgressBar));

		}

		protected virtual void ApplyViewStyles(ContextualObject view, ViewStyle style, Type viewType)
		{
			SetEnvironment(view, viewType, EnvironmentKeys.View.ClipShape, style?.ClipShape);
			SetEnvironment(view, viewType, EnvironmentKeys.View.Overlay, style?.Overlay);
			SetEnvironment(view, viewType, EnvironmentKeys.View.Border, style?.Border);
			SetEnvironment(view, viewType, EnvironmentKeys.Colors.BackgroundColor, style?.BackgroundColor);
			SetEnvironment(view, viewType, EnvironmentKeys.View.Shadow, style?.Shadow);
		}

		protected void SetEnvironment(ContextualObject view, Type type, string key, StyleAwareValue value)
		{
			if (value == null)
			{
				SetEnvironmentValue(view, type, key, null);
				return;
			}

			foreach (var pair in value.ToEnvironmentValues())
			{
				var newKey = pair.key == null ? key : $"{key}.{pair.key}";
				SetEnvironmentValue(view, type, newKey, pair.value);
			}
		}


		protected void SetEnvironment(ContextualObject view, string styleId, string key, StyleAwareValue value)
		{
			if (value == null)
			{
				SetEnvironmentValue(view, styleId, key, null);
				return;
			}
			foreach (var pair in value.ToEnvironmentValues())
			{
				var newKey = pair.key == null ? key : $"{pair.key}.{key}";
				SetEnvironmentValue(view, styleId, newKey, pair.value);
			}
		}
		protected void SetEnvironment(ContextualObject view, string styleId, string key, StyleAwareValue value, Func<object, object> getProperty)
		{
			if (value == null)
			{
				SetEnvironmentValue(view, styleId, key, null);
				return;
			}
			foreach (var pair in value.ToEnvironmentValues())
			{
				var newKey = pair.key == null ? key : $"{pair.key}.{key}";
				SetEnvironmentValue(view, styleId, newKey, getProperty(pair.value));
			}
		}


		void SetEnvironmentValue(ContextualObject view, Type type, string key, object value)
		{
			if (view != null)
				view.SetEnvironment(type, key, value);
			else
				View.SetGlobalEnvironment(type, key, value);
		}

		void SetEnvironmentValue(ContextualObject view, string styleId, string key, object value)
		{
			if (view != null)
				view.SetEnvironment(styleId, key, value);
			else
				View.SetGlobalEnvironment(styleId, key, value);
		}

		void SetDefaultControlSizingForLayouts()
		{
			void setSizing(Type control, Type container, string keyType, LayoutAlignment sizing)
			{
				var key = $"{container.Name}.{keyType}";
				SetEnvironmentValue(null, control, key, sizing);
			}
			setSizing(typeof(Text), typeof(VStack), EnvironmentKeys.Layout.HorizontalLayoutAlignment, LayoutAlignment.Fill);
			setSizing(typeof(TextField), typeof(VStack), EnvironmentKeys.Layout.HorizontalLayoutAlignment, LayoutAlignment.Fill);
			setSizing(typeof(SecureField), typeof(VStack), EnvironmentKeys.Layout.HorizontalLayoutAlignment, LayoutAlignment.Fill);
			setSizing(typeof(ProgressBar), typeof(VStack), EnvironmentKeys.Layout.HorizontalLayoutAlignment, LayoutAlignment.Fill);
			setSizing(typeof(Slider), typeof(VStack), EnvironmentKeys.Layout.HorizontalLayoutAlignment, LayoutAlignment.Fill);
			setSizing(typeof(ScrollView), typeof(HStack), EnvironmentKeys.Layout.HorizontalLayoutAlignment, LayoutAlignment.Fill);
			setSizing(typeof(ScrollView), typeof(HStack), EnvironmentKeys.Layout.VerticalLayoutAlignment, LayoutAlignment.Fill);
			setSizing(typeof(ScrollView), typeof(VStack), EnvironmentKeys.Layout.HorizontalLayoutAlignment, LayoutAlignment.Fill);
			setSizing(typeof(ScrollView), typeof(VStack), EnvironmentKeys.Layout.VerticalLayoutAlignment, LayoutAlignment.Fill);
			setSizing(typeof(WebView), typeof(HStack), EnvironmentKeys.Layout.HorizontalLayoutAlignment, LayoutAlignment.Fill);
			setSizing(typeof(WebView), typeof(HStack), EnvironmentKeys.Layout.VerticalLayoutAlignment, LayoutAlignment.Fill);
			setSizing(typeof(WebView), typeof(VStack), EnvironmentKeys.Layout.VerticalLayoutAlignment, LayoutAlignment.Fill);
			setSizing(typeof(WebView), typeof(VStack), EnvironmentKeys.Layout.HorizontalLayoutAlignment, LayoutAlignment.Fill);
			setSizing(typeof(ListView), typeof(HStack), EnvironmentKeys.Layout.HorizontalLayoutAlignment, LayoutAlignment.Fill);
			setSizing(typeof(ListView), typeof(HStack), EnvironmentKeys.Layout.VerticalLayoutAlignment, LayoutAlignment.Fill);
			setSizing(typeof(ListView), typeof(VStack), EnvironmentKeys.Layout.VerticalLayoutAlignment, LayoutAlignment.Fill);
			setSizing(typeof(ListView), typeof(VStack), EnvironmentKeys.Layout.HorizontalLayoutAlignment, LayoutAlignment.Fill);
		}
	}
}
