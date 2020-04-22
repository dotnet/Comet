using System;
using System.Collections.Generic;
using System.Text;
using System.Maui.Skia;

namespace System.Maui.Samples.Skia
{
	public class GridSample1 : View
	{
		readonly State<float> _strokeSize = 2;
		readonly State<string> _strokeColor = "#000000";

		[Body]
		View body()
		{
			var fingerPaint = new BindableFingerPaint(
				strokeSize: _strokeSize,
				strokeColor: _strokeColor);

			return new Grid(
				columns: new object[] { 120, "*" },
				rows: new object[] { 44, 44, 44, "*" })
			{
				new Label("Stroke Width:").Cell(row:0, column: 0),
				new Slider(_strokeSize, 1, 10, 1).Cell(row:0, column: 1),
				new Label("Stroke Color:").Cell(row:1, column: 0),
				new Entry(_strokeColor).Cell(row:1, column: 1),
				new Button("Reset", () => fingerPaint.Reset()).Cell(row:2, column: 0, colSpan: 2),
				fingerPaint.ToView().Cell(row:3, column: 0, colSpan: 2)
			};
		}
	}
}
