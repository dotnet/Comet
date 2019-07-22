namespace HotUI.Samples.LiveStreamIssues
{
    public class DavidSample1b : View
    {
        [Body]
        View body() =>
            new HStack(VerticalAlignment.Center)
            {
                new Spacer(),
                new ShapeView(new Circle().Stroke(Color.Black, 2f)).Frame(44,44),
                new Spacer()
            };
    }
}