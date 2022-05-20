using System;
using Microsoft.Maui;

namespace Comet.Samples
{
	public class TextWeightSample : View
	{
		[Body]
		View body() => new ScrollView{
			new VStack
			{
				new Text($"Black {(int)FontWeight.Black}").FontWeight (FontWeight.Black),
				new Text($"Heavy {(int)FontWeight.Heavy}").FontWeight (FontWeight.Heavy),
				new Text($"Bold {(int)FontWeight.Bold}").FontWeight (FontWeight.Bold),
				new Text($"Semibold {(int)FontWeight.Semibold}").FontWeight (FontWeight.Semibold),
				new Text($"Medium {(int)FontWeight.Medium}").FontWeight (FontWeight.Medium),
				new Text($"Regular {(int)FontWeight.Regular}").FontWeight (FontWeight.Regular),
				new Text($"Light {(int)FontWeight.Ultralight}").FontWeight (FontWeight.Light),
				new Text($"Ultralight {(int)FontWeight.Ultralight}").FontWeight (FontWeight.Ultralight),
				new Text($"Thin {(int)FontWeight.Thin}").FontWeight (FontWeight.Thin),
				new Text($"Black Oblique {(int)FontWeight.Black}").FontWeight (FontWeight.Black).FontSlant (FontSlant.Oblique),
				new Text($"Heavy Oblique {(int)FontWeight.Heavy}").FontWeight (FontWeight.Heavy).FontSlant (FontSlant.Oblique),
				new Text($"Bold Italic {(int)FontWeight.Bold}").FontWeight (FontWeight.Bold).FontSlant (FontSlant.Italic),
				new Text($"Semibold Oblique {(int)FontWeight.Semibold}").FontWeight (FontWeight.Semibold).FontSlant (FontSlant.Oblique),
				new Text($"Medium Oblique {(int)FontWeight.Medium}").FontWeight (FontWeight.Medium).FontSlant (FontSlant.Oblique),
				new Text($"Regular Italic {(int)FontWeight.Regular}").FontWeight (FontWeight.Regular).FontSlant (FontSlant.Italic),
				new Text($"Light Oblique {(int)FontWeight.Ultralight}").FontWeight (FontWeight.Light).FontSlant (FontSlant.Oblique),
				new Text($"Ultralight Oblique {(int)FontWeight.Ultralight}").FontWeight (FontWeight.Ultralight).FontSlant (FontSlant.Oblique),
				new Text($"Thin Oblique {(int)FontWeight.Thin}").FontWeight (FontWeight.Thin).FontSlant (FontSlant.Oblique),
			}
		};
	}
}
