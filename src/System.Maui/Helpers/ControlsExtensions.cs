using System;
using System.Maui.Internal;
namespace System.Maui
{
	public static class ControlsExtensions
	{
		public static float GetPercent(this Slider slider)
		{
			var end = slider.Through?.CurrentValue ?? 0;
			var current = slider.Value?.CurrentValue ?? 0;
			return current.SafeDivideByZero(end);
		}
	}
}
