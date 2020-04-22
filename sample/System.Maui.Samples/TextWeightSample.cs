using System;

namespace System.Maui.Samples
{
	public class TextWeightSample : View
	{
		[Body]
		View body() => new VStack
		{
			new Label($"Black {(int)Weight.Black}").FontWeight (Weight.Black),
			new Label($"Heavy {(int)Weight.Heavy}").FontWeight (Weight.Heavy),
			new Label($"Bold {(int)Weight.Bold}").FontWeight (Weight.Bold),
			new Label($"Semibold {(int)Weight.Semibold}").FontWeight (Weight.Semibold),
			new Label($"Medium {(int)Weight.Medium}").FontWeight (Weight.Medium),
			new Label($"Regular {(int)Weight.Regular}").FontWeight (Weight.Regular),
			new Label($"Light {(int)Weight.Ultralight}").FontWeight (Weight.Light),
			new Label($"Ultralight {(int)Weight.Ultralight}").FontWeight (Weight.Ultralight),
			new Label($"Thin {(int)Weight.Thin}").FontWeight (Weight.Thin),
			new Label($"Black Italic {(int)Weight.Black}").FontWeight (Weight.Black).FontItalic (true),
			new Label($"Heavy Italic {(int)Weight.Heavy}").FontWeight (Weight.Heavy).FontItalic (true),
			new Label($"Bold Italic {(int)Weight.Bold}").FontWeight (Weight.Bold).FontItalic (true),
			new Label($"Semibold Italic Italic {(int)Weight.Semibold}").FontWeight (Weight.Semibold).FontItalic (true),
			new Label($"Medium Italic {(int)Weight.Medium}").FontWeight (Weight.Medium).FontItalic (true),
			new Label($"Regular Italic {(int)Weight.Regular}").FontWeight (Weight.Regular).FontItalic (true),
			new Label($"Light Italic {(int)Weight.Ultralight}").FontWeight (Weight.Light).FontItalic (true),
			new Label($"Ultralight Italic {(int)Weight.Ultralight}").FontWeight (Weight.Ultralight).FontItalic (true),
			new Label($"Thin Italic {(int)Weight.Thin}").FontWeight (Weight.Thin).FontItalic (true),
		};
	}
}
