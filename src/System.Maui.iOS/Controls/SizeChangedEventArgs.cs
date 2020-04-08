using System;
using CoreGraphics;

namespace System.Maui.iOS.Controls
{
	public class SizeChangedEventArgs : EventArgs
	{
		public SizeChangedEventArgs(CGSize size)
		{
			Size = size;
		}

		public CGSize Size { get; }

	}
}
