//using FluentButton = Comet.Skia.Fluent.Button;

namespace Comet.Samples.Skia
{
	public class SkiaControlSample : View
	{
		readonly State<int> count = 0;

		[Body]
		View body() => new VStack
		{
			new Button(() => $"Hello form the platform {count.Value}", () => ++count.Value),
            //new FluentButton($"Hello from skia {count.Value}", () => ++count.Value)
        };
	}
}
