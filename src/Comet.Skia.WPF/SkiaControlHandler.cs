using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comet.Skia.WPF
{
	public class SkiaControlHandler<T> : SkiaViewHandler
	   where T : SkiaControl, new()
	{
		readonly T control;
		public SkiaControlHandler()
		{
			control = new T();
		}
		public override void SetView(View view)
		{
			control.SetView(view);
			base.SetView(control);
		}
		public override SizeF GetIntrinsicSize(SizeF availableSize) => control.GetIntrinsicSize(availableSize);
		public override void UpdateValue(string property, object value)
		{
			if (control is IViewHandler viewHandler)
				viewHandler.UpdateValue(property, value);
			base.UpdateValue(property, value);
		}
	}
}
