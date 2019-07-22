namespace HotUI.Samples.LiveStreamIssues
{
    public class DavidSample1a : View
    {
        [Body]
        View body() =>
            new VStack(HorizontalAlignment.Center)
            {
                new ShapeView(new Circle().Stroke(Color.Black, 2f)).Frame(44,44)
            };
    }
}