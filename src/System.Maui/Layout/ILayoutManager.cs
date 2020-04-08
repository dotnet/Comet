using System.Drawing;

namespace System.Maui.Layout
{
	public interface ILayoutManager
	{
		void Invalidate();
		SizeF Measure(AbstractLayout layout, SizeF available);
		void Layout(AbstractLayout layout, RectangleF rect);
	}
}
