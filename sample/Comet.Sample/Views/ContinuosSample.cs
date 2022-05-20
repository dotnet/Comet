using System;
namespace Comet.Samples
{
	public class ContinuosSample : View
	{
		readonly State<string> _strokeColor = "#000000";

		[Body]
		View body()
		{

			return new Grid(
				columns: new object[] { "*", "*" },
				rows: null)
			{
				new TextField(_strokeColor, "Enter code here").Cell(row:0, column: 0),
				new Button("Controls appear here").Cell(row:0, column:1)
			};
		}
	}
}
