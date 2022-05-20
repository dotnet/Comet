//using System;
//namespace Comet.Samples
//{
//	public class SkiaButtonSample : View
//	{
//		[Body]
//		View body() => new VStack
//		{
//			new SKText("Hello").Color(Colors.Black)
//			.BeginAnimationSequence(repeats:true)
//					.Animate(duration:1,action:(text)=>{
//						text.Color(Color.Fuchsia);
//					}).Animate(duration:1,action:(text)=>{
//						text.Color(Color.AliceBlue);
//					}).Animate(duration:1,action:(text)=>{
//						text.Color(Color.Beige);
//					}).Animate(duration:1,action:(text)=>{
//						text.Color(Color.BlueViolet);
//					}).Animate(duration:1,action:(text)=>{
//						text.Color(Color.Lavender);
//					}).Animate(duration:1,action:(text)=>{
//						text.Color(Color.Fuchsia);
//				}).EndAnimationSequence(),
//			//TODO: Figure out why animation doesn't work
//			new SKButton("Hello").RoundedBorder(color:Colors.Black).Background(Colors.Black).Color(Colors.White).Animate(x => {
//				x.Background(Colors.White).Color(Colors.Black);
//			},duration:3, autoReverses:true, repeats: true)
//		};
//	}
//}
