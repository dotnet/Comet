using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using UWPTextAlignment = Windows.UI.Xaml.TextAlignment;
using Comet.Internal;

namespace Comet.UWP
{
	public static class UWPExtensions
	{
		static UWPExtensions()
		{
			UI.Init();
		}

		public static UWPViewHandler GetOrCreateViewHandler(this View view)
		{
			if (view == null)
				return null;
			var handler = view.ViewHandler;
			if (handler == null)
			{
				var builtView = view.GetView();
				handler = Registrar.Handlers.GetHandler(builtView.GetType());
				builtView.ViewHandler = handler;
			}

			var iUIElement = handler as UWPViewHandler;
			return iUIElement;
		}

		public static UIElement ToView(this View view)
		{
			var handler = view.GetOrCreateViewHandler();
			return handler?.View;
		}

		public static Color ToColor(this Windows.UI.Color color)
		{
			if (color == null)
				return null;
			return new Color((float)(color.R / 255f), (float)(color.G / 255f), (float)(color.B / 255f), (float)(color.A / 255f));
		}

		public static Windows.UI.Color FromColor(this Color color)
		{
			return new Windows.UI.Color
			{
				R = (byte)(color.R * 255f),
				G = (byte)(color.G * 255f),
				B = (byte)(color.B * 255f),
				A = (byte)(color.A * 255f),
			};
		}

		public static Weight ToWeight(this ushort weight)
		{
			switch ((int)weight)
			{
				case (int)Weight.Regular:
					return Weight.Regular;
				case (int)Weight.Thin:
					return Weight.Thin;

				case (int)Weight.Ultralight:
					return Weight.Ultralight;

				case (int)Weight.Light:
					return Weight.Light;

				case (int)Weight.Medium:
					return Weight.Medium;

				case (int)Weight.Semibold:
					return Weight.Semibold;

				case (int)Weight.Bold:
					return Weight.Bold;

				case (int)Weight.Heavy:
					return Weight.Heavy;

				case (int)Weight.Black:
					return Weight.Black;
				default:
					return Weight.Regular;
			}
		}

		public static FontAttributes ToFont(this TextBlock textBlock)
		{
			if (textBlock == null)
				return null;
			var fontAttributes = new FontAttributes
			{
				Italic = textBlock.FontStyle == Windows.UI.Text.FontStyle.Italic,
				Size = (float)textBlock.FontSize,
				Weight = textBlock.FontWeight.Weight.ToWeight(),
				Family = textBlock.FontFamily.Source
			};
			return fontAttributes;
		}

		public static void SetFont(this TextBlock textBlock, FontAttributes fontAttributes)
		{
			if (textBlock == null)
				return;
			textBlock.FontStyle = fontAttributes.Italic ? Windows.UI.Text.FontStyle.Italic : Windows.UI.Text.FontStyle.Normal;
			textBlock.FontSize = fontAttributes.Size;
			textBlock.FontWeight = new Windows.UI.Text.FontWeight
			{
				Weight = (ushort)fontAttributes.Weight
			};
			textBlock.FontFamily = new Windows.UI.Xaml.Media.FontFamily(fontAttributes.Family);
		}

		public static void SetFontColor(this TextBlock textBlock, Color color)
		{
			if (textBlock == null)
				return;
			textBlock.Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(color.FromColor());
		}

		public static UIElement ToEmbeddableView(this View view)
		{
			var handler = view.GetOrCreateViewHandler();
			if (handler == null)
				throw new Exception("Unable to build handler for view");

			if (handler is FrameworkElement element)
			{
				if (element.Parent is CometView container)
					return container;
			}

			return new CometView(view);
		}

		public static void RemoveChild(this DependencyObject parent, UIElement child)
		{
			if (parent is Panel panel)
			{
				panel.Children.Remove(child);
				return;
			}

			/*var decorator = parent as Decorator;
            if (decorator != null)
            {
                if (decorator.Child == child)
                {
                    decorator.Child = null;
                }
                return;
            }*/

			if (parent is ContentPresenter contentPresenter)
			{
				if (contentPresenter.Content == child)
				{
					contentPresenter.Content = null;
				}
				return;
			}

			if (parent is ContentControl contentControl)
			{
				if (contentControl.Content == child)
				{
					contentControl.Content = null;
				}
				return;
			}
		}

		public static UWPTextAlignment ToTextAlignment(this TextAlignment? target)
		{
			if (target == null)
				return UWPTextAlignment.Start;

			switch (target)
			{
				case TextAlignment.Natural:
					return UWPTextAlignment.Start;
				case TextAlignment.Left:
					return UWPTextAlignment.Left;
				case TextAlignment.Right:
					return UWPTextAlignment.Right;
				case TextAlignment.Center:
					return UWPTextAlignment.Center;
				case TextAlignment.Justified:
					return UWPTextAlignment.Justify;
				default:
					throw new ArgumentOutOfRangeException(nameof(target), target, null);
			}
		}
	}
}
