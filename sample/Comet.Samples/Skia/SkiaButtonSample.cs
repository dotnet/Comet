using System;
using Comet.Skia;
namespace Comet.Samples
{
    public class SkiaButtonSample : View
    {
        [Body]
        View body() => new VStack
        {
			new SKText("Hello").Color(Color.Black)
			.Animate(x=> x.Color(Color.Blue),duration:3),
			//TODO: Figure out why animation doesn't work
			new SKButton("Hello").Background(Color.Black).Color(Color.White).Animate(x => {
                x.Background(Color.White).Color(Color.Black);
            },duration:3)
        };
    }
}
