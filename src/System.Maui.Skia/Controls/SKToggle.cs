using System;
namespace System.Maui.Skia
{
	public class SKToggle : Switch
	{
		public SKToggle()
		{
		}
		public SKToggle(
			Binding<bool> value = null,
			Action<bool> onChanged = null) : base(value, onChanged) { }
	}
}
