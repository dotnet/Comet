namespace HotUI.Samples
{
    public class AnimationSample : View
    {
        public AnimationSample()
        {
            Body = Build;
        }

        View Build() =>
            new Text("Text to Animate!")
                .Background(Color.Orange)
                .Animate(new Animation
                {
                    Duration = 2000,
                    Delay = 500,
                    Options = AnimationOptions.CurveEaseOut,// | AnimationOptions.Repeat,
                    TranslateTo = new PointF(100, 100),
                    //RotateTo = 90,
                });
    }
}
