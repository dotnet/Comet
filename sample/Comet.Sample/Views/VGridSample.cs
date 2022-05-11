namespace Comet.Samples;

public class VGridSample : View
{
	[Body]
	View view() => new VGrid(4)
	{
		Enumerable.Range(0,20).Select(x=>
			new Text($"{x}")
				.HorizontalTextAlignment(TextAlignment.Center)
		),
	};
}