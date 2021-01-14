using System.Graphics;

namespace Comet.Samples
{
	public class AnimationSample : View
	{
		public AnimationSample()
		{
			Body = Build;
		}
		readonly State<bool> shouldAnimate = true;
		Text animatedText;
		Button button;
		View Build() =>
			new VStack
			{
				new Text("Regular Text Behind..."),
				(animatedText = new Text("Text to Animate!")
					.Background(Color.Orange)
					.Color(Color.Blue)
					.FontSize(10)
					.Animate(duration: 3,repeats:true, autoReverses:true, action: (text) => {
						text.Background(Color.Blue)
							.Color(Color.Orange);
						text.FontSize(30);
					})

					//new Animation
					//{
					//	Duration = 2000,
					//	Delay = 500,
					//	Options = AnimationOptions.CurveEaseOut | AnimationOptions.Repeat,
					//	TranslateTo = new PointF(100, 50),
					//	RotateTo = 30,
					//	ScaleTo = new PointF(2f, 2f),
					//}
					),
				new Text("Regular Text Above...")
				.BeginAnimationSequence(repeats:true)
					.Animate(duration:1,action:(text)=>{
						text.Background(Color.Fuchsia);
						button.Background(Color.Green);
					}).Animate(duration:1,action:(text)=>{
						text.Background(Color.AliceBlue);
					}).Animate(duration:1,action:(text)=>{
						text.Background(Color.Beige);
						button.Background(Color.Blue);
					}).Animate(duration:1,action:(text)=>{
						text.Background(Color.BlueViolet);
					}).Animate(duration:1,action:(text)=>{
						text.Background(Color.Lavender);
					}).Animate(duration:1,action:(text)=>{
						text.Background(Color.Fuchsia);
						button.Background(Color.Green);
				}).EndAnimationSequence(),
				new Text("Does this move?").Background(Color.LightBlue).Frame(width:100,height:44).Animate((text)=>{
					text.Frame(width:400, height: 100);
				},duration:3,repeats:true,autoReverses:true),
				(button = new Button("Animate", () => {
					shouldAnimate.Value = !shouldAnimate;
					if(shouldAnimate)
						this.ResumeAnimations();
					else
						this.PauseAnimations();
				}))
			};
	}
}
