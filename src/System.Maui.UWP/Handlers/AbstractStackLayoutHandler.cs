using System;
using System.Drawing;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace System.Maui.UWP.Handlers
{
	public abstract class AbstractStackLayoutHandler : StackPanel, UWPViewHandler
	{
		private AbstractLayout _view;

		public event EventHandler<ViewChangedEventArgs> NativeViewChanged;

		public UIElement View => this;

		public object NativeView => View;

		public bool HasContainer
		{
			get => false;
			set { }
		}

		public SizeF GetIntrinsicSize(SizeF availableSize)
		{
			Measure(availableSize.ToWSize());
			return DesiredSize.ToSizeF();
		}

		public void SetFrame(RectangleF frame)
		{
			Arrange(frame.ToRect());
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
					var nativeView = subView.ToView();
					Children.Add(nativeView);
				}
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
				var nativeView = view.ToView();
				Children.Insert(index, nativeView);
			}
		}

		private void ViewOnChildrenRemoved(object sender, LayoutEventArgs e)
		{
			for (var i = 0; i < e.Count; i++)
			{
				var index = e.Start + i;
				Children.RemoveAt(index);
			}
		}

		private void HandleChildrenChanged(object sender, LayoutEventArgs e)
		{
			for (var i = 0; i < e.Count; i++)
			{
				var index = e.Start + i;
				Children.RemoveAt(index);

				var view = _view[index];
				var newNativeView = view.ToView();
				Children.Insert(index, newNativeView);
			}
		}
	}
}
