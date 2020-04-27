using System;

namespace Comet.Samples
{
	public class TextWeightSample : View
	{
		[Body]
		View body() => new VStack
		{
			new Text($"Black {(int)Weight.Black}").FontWeight (Weight.Black),
			new Text($"Heavy {(int)Weight.Heavy}").FontWeight (Weight.Heavy),
			new Text($"Bold {(int)Weight.Bold}").FontWeight (Weight.Bold),
			new Text($"Semibold {(int)Weight.Semibold}").FontWeight (Weight.Semibold),
			new Text($"Medium {(int)Weight.Medium}").FontWeight (Weight.Medium),
			new Text($"Regular {(int)Weight.Regular}").FontWeight (Weight.Regular),
			new Text($"Light {(int)Weight.Ultralight}").FontWeight (Weight.Light),
			new Text($"Ultralight {(int)Weight.Ultralight}").FontWeight (Weight.Ultralight),
			new Text($"Thin {(int)Weight.Thin}").FontWeight (Weight.Thin),
			new Text($"Black Italic {(int)Weight.Black}").FontWeight (Weight.Black).FontItalic (true),
			new Text($"Heavy Italic {(int)Weight.Heavy}").FontWeight (Weight.Heavy).FontItalic (true),
			new Text($"Bold Italic {(int)Weight.Bold}").FontWeight (Weight.Bold).FontItalic (true),
			new Text($"Semibold Italic Italic {(int)Weight.Semibold}").FontWeight (Weight.Semibold).FontItalic (true),
			new Text($"Medium Italic {(int)Weight.Medium}").FontWeight (Weight.Medium).FontItalic (true),
			new Text($"Regular Italic {(int)Weight.Regular}").FontWeight (Weight.Regular).FontItalic (true),
			new Text($"Light Italic {(int)Weight.Ultralight}").FontWeight (Weight.Light).FontItalic (true),
			new Text($"Ultralight Italic {(int)Weight.Ultralight}").FontWeight (Weight.Ultralight).FontItalic (true),
			new Text($"Thin Italic {(int)Weight.Thin}").FontWeight (Weight.Thin).FontItalic (true),
		};
	}
}
