using System;
using Comet.Skia;
namespace Comet.Samples
{
	public class SkiaControlsSample : View
	{
		[Body]
		View body() => new VStack
		{
			new SKText("Text"),
			new SKButton("Hello!",()=> this.Dismiss()),
			new SKSlider(),
		};
	}
}
