using System;
using AppKit;
using System.Maui.Mac.Extensions;

namespace System.Maui.Mac.Controls
{
	public class CUITableViewCell : NSTableCellView
	{
		private NSView _nativeView;
		private View _virtualView;

		public void SetView(View view)
		{
			var oldView = _virtualView;
			var isFromThisCell = oldView?.ToView() == _nativeView;
			//Apple bug, somehow the view be a weird recycle... So only re-use if the view still matches
			if (isFromThisCell && _virtualView != null && !_virtualView.IsDisposed)
			{
				view = view.Diff(_virtualView,false);
			}

			_virtualView = view;
			var newView = view.ToView();

			if (_nativeView != null && _nativeView != newView)
			{
				_nativeView.RemoveFromSuperview();
			}

			_nativeView = newView;
			if (_nativeView != null && _nativeView.Superview != this)
				AddSubview(_nativeView);
		}

		public override void Layout()
		{
			base.Layout();
			if (_nativeView == null) return;

			_virtualView.SetFrameFromNativeView(Bounds.ToRectangleF());
		}
	}
}
