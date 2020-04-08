using System;
using System.Drawing;

namespace System.Maui
{
	public interface IViewHandler //TODO: UnComment this: IDisposable
	{
		void SetView(View view);
		void UpdateValue(string property, object value);
		void Remove(View view);
		object NativeView { get; }
		bool HasContainer { get; set; }
		SizeF GetIntrinsicSize(SizeF availableSize);
		void SetFrame(RectangleF frame);
	}
}
