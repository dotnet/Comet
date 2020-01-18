using System;
namespace Comet.Skia
{
	public class SKToggle : Toggle
	{
		public SKToggle()
		{
		}
		public SKToggle(
			Binding<bool> value = null,
			Action<bool> onChanged = null) : base(value, onChanged) { }
	}
}
