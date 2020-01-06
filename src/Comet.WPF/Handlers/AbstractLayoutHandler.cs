using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using WPFSize = System.Windows.Size;

namespace Comet.WPF.Handlers
{
	public abstract class AbstractLayoutHandler : Panel, WPFViewHandler
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
					InternalChildren.Add(nativeView);
				}

				InvalidateMeasure();
				InvalidateArrange();
			}
		}

		public void Remove(View view)
		{
			InternalChildren.Clear();

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
				var nativeView = view.ToView();
				InternalChildren.Insert(index, nativeView);
			}

			InvalidateMeasure();
			InvalidateArrange();
		}

		private void ViewOnChildrenRemoved(object sender, LayoutEventArgs e)
		{
			for (var i = 0; i < e.Count; i++)
			{
				var index = e.Start + i;
				InternalChildren.RemoveAt(index);
			}

			InvalidateMeasure();
			InvalidateArrange();
		}

		private void HandleChildrenChanged(object sender, LayoutEventArgs e)
		{
			for (var i = 0; i < e.Count; i++)
			{
				var index = e.Start + i;
				InternalChildren.RemoveAt(index);

				var view = _view[index];
				var newNativeView = view.ToView();
				InternalChildren.Insert(index, newNativeView);
			}

			InvalidateMeasure();
			InvalidateArrange();
		}

		protected override WPFSize MeasureOverride(WPFSize availableSize)
		{
			return _view.Measure(availableSize.ToSizeF()).ToWSize();
		}

		protected override WPFSize ArrangeOverride(WPFSize finalSize)
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
