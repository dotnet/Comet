using System.Drawing;

namespace System.Maui.Layout
{
	public class ZStackLayoutManager : ILayoutManager
	{
		public void Invalidate()
		{

		}

		public SizeF Measure(AbstractLayout layout, SizeF available)
		{
			return available;
		}

		public void Layout(AbstractLayout layout, RectangleF rect)
		{
			foreach (var v in layout)
			{
				v.Frame = rect;
			}
		}
	}
}
