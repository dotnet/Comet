using System;
namespace Comet.Skia
{
	public class SKSlider : Slider
	{
		public SKSlider(
		 Binding<float> value = null,
		 float from = 0,
		 float through = 100,
		 float by = 1,
		 Action<float> onEditingChanged = null)
			: base(value, from, through, by, onEditingChanged)
		{ }
	}
}
