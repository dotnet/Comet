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
