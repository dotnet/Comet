using System.Drawing;

namespace Comet.Samples
{
	public class AnimationSample : View
	{
		public AnimationSample()
		{
			Body = Build;
		}
		Text animatedText;
		View Build() =>
			new VStack
			{
				new Text("Regular Text Behind..."),
				(animatedText = new Text("Text to Animate!")
					.Background(Color.Orange)
					.Animate(duration: 3,repeats:true, autoReverses:true, action: (text) => {
						text.Background(Color.Blue);
					}
					//new Animation
					//{
					//	Duration = 2000,
					//	Delay = 500,
					//	Options = AnimationOptions.CurveEaseOut | AnimationOptions.Repeat,
					//	TranslateTo = new PointF(100, 50),
					//	RotateTo = 30,
					//	ScaleTo = new PointF(2f, 2f),
					//}
					)),
				new Text("Regular Text Above..."),
				new Button("Animate", () => {
					animatedText.Animate(duration: 3, action: (text) => {
						text.Background(Color.Pink);
					});
				})
			};
	}
}
