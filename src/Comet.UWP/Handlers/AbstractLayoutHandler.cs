using System;
using System.Drawing;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using UwpSize = Windows.Foundation.Size;

namespace Comet.UWP.Handlers
{
	public abstract class AbstractLayoutHandler : Panel, UWPViewHandler
	{
		private AbstractLayout _view;
		private bool _inLayout;

		public event EventHandler<ViewChangedEventArgs> NativeViewChanged;

		public UIElement View => this;

		public object NativeView => View;

		public bool HasContainer
		{
			get => false;
			set { }
		}

		public SizeF GetIntrinsicSize(SizeF availableSize) => Comet.View.UseAvailableWidthAndHeight;

		public void SetFrame(RectangleF frame)
		{
			_inLayout = true;
			Arrange(frame.ToRect());
			_inLayout = false;
		}

		public void SetView(View view)
		{
			_view = view as AbstractLayout;
			if (_view != null)
			{
				_view.ChildrenChanged += HandleChildrenChanged;
				_view.ChildrenAdded += HandleChildrenAdded;
				_view.ChildrenRemoved += ViewOnChildrenRemoved;

				foreach (var subView in _view)
				{
					var nativeView = subView.ToView() ?? new Canvas();
					Children.Add(nativeView);
				}

				InvalidateMeasure();
			}
		}

		public void Remove(View view)
		{
			Children.Clear();

			if (view != null)
			{
				_view.ChildrenChanged -= HandleChildrenChanged;
				_view.ChildrenAdded -= HandleChildrenAdded;
				_view.ChildrenRemoved -= ViewOnChildrenRemoved;
				_view = null;
			}
		}

		public virtual void UpdateValue(string property, object value)
		{
		}

		private void HandleChildrenAdded(object sender, LayoutEventArgs e)
		{
			for (var i = 0; i < e.Count; i++)
			{
				var index = e.Start + i;
				var view = _view[index];
				var nativeView = view.ToView() ?? new Canvas();
				Children.Insert(index, nativeView);
			}

			InvalidateMeasure();
		}

		private void ViewOnChildrenRemoved(object sender, LayoutEventArgs e)
		{
			for (var i = 0; i < e.Count; i++)
			{
				var index = e.Start + i;
				Children.RemoveAt(index);
			}

			InvalidateMeasure();
		}

		private void HandleChildrenChanged(object sender, LayoutEventArgs e)
		{
			for (var i = 0; i < e.Count; i++)
			{
				var index = e.Start + i;
				Children.RemoveAt(index);

				var view = _view[index];
				var newNativeView = view.ToView() ?? new Canvas();
				Children.Insert(index, newNativeView);
			}

			InvalidateMeasure();
		}

		protected override UwpSize MeasureOverride(UwpSize availableSize)
		{
			return _view?.Measure(availableSize.ToSizeF()).ToWSize() ?? availableSize;
		}

		protected override UwpSize ArrangeOverride(UwpSize finalSize)
		{
			if (_inLayout) return finalSize;

			if (finalSize.Width > 0 && finalSize.Height > 0)
			{
				_view.Frame = RectangleF.Empty;
				_view.Frame = new RectangleF(0, 0, (float)finalSize.Width, (float)finalSize.Height);
				_view.LayoutSubviews(_view.Frame);
			}

			return finalSize;
		}
	}
}
