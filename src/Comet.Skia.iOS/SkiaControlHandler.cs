using System;
using System.Drawing;

namespace Comet.Skia.iOS
{
	public class SkiaControlHandler<T> : SkiaViewHandler
		where T : SkiaControl, new()
	{
		T control;
		public SkiaControlHandler()
		{
			control = new T();
		}
		public override void SetView(View view)
		{
			control.SetView(view);
			base.SetView(control);
		}
		public override SizeF Measure(SizeF availableSize) => control.Measure(availableSize);
	}
}
