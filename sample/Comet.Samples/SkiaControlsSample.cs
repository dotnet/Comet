using System;
using Comet.Skia;
namespace Comet.Samples
{
	public class SkiaControlsSample : View
	{
		readonly State<float> progress = .5f;
		[Body]
		View body() => new VStack
		{
			new SKText("Text"),
			new SKTextField("Text Field"),
			new SKButton("Hello!"),
			new SKSlider(progress,0,1,.01f),
			new SKProgressBar(progress),
			new SKToggle(),
		};
	}
}
