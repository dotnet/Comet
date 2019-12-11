using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comet.Skia.UWP
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
