using System;
using System.Drawing;

namespace Comet.Skia
{
	public class SliderHandler : SKiaAbstractControlHandler<Slider>
	{
		public SliderHandler()
		{
		}

		public override string AccessibilityText() => TypedVirtualView.Value.CurrentValue.ToString("P");
	}
}
