using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WGrid = Microsoft.UI.Xaml.Controls.Grid;
using UwpSize = Windows.Foundation.Size;

namespace Comet.Platform.Windows
{
	class ListCell : WGrid
	{
		private View _view;
		private UIElement _nativeView;
		private IElementHandler _handler;

		public ListCell(View view , IMauiContext context)
		{
			Context = context;
			View = view;
		}

		public View View
		{
			get => _view;
			set
			{
				if (value == _view)
					return;

				_view = value;
				_handler = _view?.ViewHandler;
				UpdateView();
			}
		}

		public IMauiContext Context { get; }

		private void LayoutChanged(object sender, EventArgs e)
		{
			InvalidateMeasure();
			InvalidateArrange();
		}

		private void UpdateView()
		{
			if (_nativeView != null)
			{
				Children.Remove(_nativeView);
				_nativeView = null;
			}

			_nativeView = _view?.ToPlatform(Context);

			if (_nativeView != null)
			{
				if (_nativeView is FrameworkElement frameworkElement)
				{
					WGrid.SetRow(frameworkElement, 0);
					WGrid.SetColumn(frameworkElement, 0);
					WGrid.SetColumnSpan(frameworkElement, 1);
					WGrid.SetRowSpan(frameworkElement, 1);
				}

				Children.Add(_nativeView);
			}
		}

		protected override UwpSize MeasureOverride(UwpSize availableSize)
		{
			var measuredSize = _view?.Measure(availableSize.Width, availableSize.Height).ToPlatform();
			return measuredSize ?? availableSize;
		}

		protected override UwpSize ArrangeOverride(UwpSize finalSize)
		{
			if (finalSize.Width > 0 && finalSize.Height > 0 && _view != null)
				_view.Frame = new RectangleF(0, 0, (float)finalSize.Width, (float)finalSize.Height);

			return finalSize;
		}
	}
}
