namespace Comet.Samples;

public class VGridNumberPad : View
{
	[Body]
	View view() => new VGrid(3)
	{
		new Button("7"),new Button("8"),new Button("9"),
		new Button("4"),new Button("5"),new Button("6"),
		new Button("1"),new Button("2"),new Button("3"),
		new Button("0").NextColumn()
	};
}
