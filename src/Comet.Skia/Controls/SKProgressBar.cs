using System;
namespace Comet.Skia
{
	public class SKProgressBar : ProgressBar
	{
		public SKProgressBar( Binding<float> value = null) : base(value) { }

		public SKProgressBar( Func<float> value) : base(value){ }
	}
}
