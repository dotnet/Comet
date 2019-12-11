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
			.BeginAnimationSequence(repeats:true)
					.Animate(duration:1,action:(text)=>{
						text.Color(Color.Fuchsia);
					}).Animate(duration:1,action:(text)=>{
						text.Color(Color.AliceBlue);
					}).Animate(duration:1,action:(text)=>{
						text.Color(Color.Beige);
					}).Animate(duration:1,action:(text)=>{
						text.Color(Color.BlueViolet);
					}).Animate(duration:1,action:(text)=>{
						text.Color(Color.Lavender);
					}).Animate(duration:1,action:(text)=>{
						text.Color(Color.Fuchsia);
				}).EndAnimationSequence(),
			//TODO: Figure out why animation doesn't work
			new SKButton("Hello").RoundedBorder(color:Color.Black).Background(Color.Black).Color(Color.White).Animate(x => {
                x.Background(Color.White).Color(Color.Black);
            },duration:3, autoReverses:true, repeats: true)
        };
    }
}
