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
			new Button("Hello!"),
			new Slider(progress,0,1,.01f),
			new ProgressBar(progress),
			new Toggle(),
		};
	}
}
