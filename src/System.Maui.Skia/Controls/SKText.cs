using System;
namespace System.Maui.Skia
{
	public class SKText : Label
	{
		public SKText(
			Binding<string> value = null) : base(value)
		{

		}
		public SKText(
			Func<string> value) : this((Binding<string>)value)
		{

		}

	}
}
