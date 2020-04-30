using System;
using System.Windows;
using System.Windows.Controls;
using WPFTextAlignment = System.Windows.HorizontalAlignment;
using Comet.Internal;

namespace Comet.WPF
{
	public static class WPFExtensions
	{
		static WPFExtensions()
		{
			UI.Init();
		}

		public static WPFViewHandler GetOrCreateViewHandler(this View view)
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

			var iUIElement = handler as WPFViewHandler;
			return iUIElement;
		}

		public static double GetWpfFontSize(this Text virtualText)
		{
			var styleId = virtualText.StyleId;
			switch (styleId)
			{
				case EnvironmentKeys.Text.Style.H1:
					return 32;
				case EnvironmentKeys.Text.Style.H2:
					return 24;
				case EnvironmentKeys.Text.Style.H3:
					return 18.72;
				case EnvironmentKeys.Text.Style.H4:
					return 16;
				case EnvironmentKeys.Text.Style.H5:
					return 13.27;
				case EnvironmentKeys.Text.Style.H6:
					return 10.72;
				case EnvironmentKeys.Text.Style.Body1:
					return 12;
				case EnvironmentKeys.Text.Style.Body2:
					return 10.50;
				case EnvironmentKeys.Text.Style.Caption:
					return 9;
				case EnvironmentKeys.Text.Style.Subtitle1:
					return 10;
				case EnvironmentKeys.Text.Style.Subtitle2:
					return 10.50;
				case EnvironmentKeys.Text.Style.Overline:
					return 7;
				default:
					return 12;
			}
		}

		public static FontWeight GetWpfFontWeight(this Weight virtualTextWight)
		{
			switch(virtualTextWight)
			{
				case Weight.Bold:
					return FontWeights.Bold;
				case Weight.Heavy:
					return FontWeights.Heavy;
				case Weight.Black:
					return FontWeights.Black;
				case Weight.Light:
					return FontWeights.Light;
				case Weight.Medium:
					return FontWeights.Medium;
				case Weight.Semibold:
					return FontWeights.SemiBold;
				case Weight.Thin:
					return FontWeights.Thin;
				case Weight.Ultralight:
					return FontWeights.UltraLight;
				default:
					return FontWeights.Regular;
			}
		}

		public static UIElement ToView(this View view)
		{
			var handler = view.GetOrCreateViewHandler();
			return handler?.View;
		}

		public static UIElement ToEmbeddableView(this View view)
		{
			var handler = view.GetOrCreateViewHandler();
			if (handler == null)
				throw new Exception("Unable to build handler for view");

			return new CometContainerView(view);
		}

		public static void RemoveChild(this DependencyObject parent, UIElement child)
		{
			if (parent is Panel panel)
			{
				panel.Children.Remove(child);
				return;
			}

			if (parent is Decorator decorator)
			{
				if (decorator.Child == child)
				{
					decorator.Child = null;
				}
				return;
			}

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

		public static WPFTextAlignment ToHorizontalAlignment(this TextAlignment? target)
		{
			// todo: this needs to add support for RTL
			if (target == null)
				return WPFTextAlignment.Left;

			switch (target)
			{
				case TextAlignment.Natural:
					return WPFTextAlignment.Left;
				case TextAlignment.Left:
					return WPFTextAlignment.Left;
				case TextAlignment.Right:
					return WPFTextAlignment.Right;
				case TextAlignment.Center:
					return WPFTextAlignment.Center;
				case TextAlignment.Justified:
					return WPFTextAlignment.Stretch;
				default:
					throw new ArgumentOutOfRangeException(nameof(target), target, null);
			}
		}
	}
}
