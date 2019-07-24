namespace HotUI.Blazor.Sample
{
    public class Sample1 : View
    {
        readonly State<int> _counter = 0;

        [Body]
        View body() => new VStack
        {
            new Text(() => $"Current count: {_counter.Value}"),
            new Button("Click me", () => _counter.Value++)
        };
    }
}
