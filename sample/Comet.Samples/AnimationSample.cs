namespace Comet.Samples
{
    public class AnimationSample : View
    {
        public AnimationSample()
        {
            Body = Build;
        }

        View Build() =>
            new VStack
            {
                new Text("Regular Text Behind..."),
                new Text("Text to Animate!")
                    .Background(Color.Orange)
                    .Animate(new Animation
                    {
                        Duration = 2000,
                        Delay = 500,
                        Options = AnimationOptions.CurveEaseOut | AnimationOptions.Repeat,
                        TranslateTo = new PointF(100, 50),
                        RotateTo = 30,
                        ScaleTo = new PointF(2f, 2f),
                    }),
                new Text("Regular Text Above..."),
            };
    }
}
