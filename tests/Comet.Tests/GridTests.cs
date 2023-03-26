using System;
using Comet.Tests.Handlers;
using Microsoft.Maui.Graphics;
using Xunit;

namespace Comet.Tests
{
	public class GridTests : TestBase
	{
		public class GridTestView1 : View
		{
			public readonly State<string> text = "Test";

			[Body]
			View body() => new Grid(columns: new[] { "*", "*" }, defaultRowHeight: 20)
				{
					new TextField(text).Tag("textfield").Cell(row:0, column:0, colSpan:2),
					new Text(text).Tag("text1").Cell(row:1, column: 0),
					new Text(text).Tag("text2").Cell(row:1, column: 1),
				}.Tag("grid");
		}

		[Fact(Skip ="Needs Fixing")]
		public void TestView1()
		{
			var view = new GridTestView1();
			InitializeHandlers(view);

			var grid = view.GetViewWithTag<Grid>("grid");
			var textField = view.GetViewWithTag<TextField>("textfield");
			var text1 = view.GetViewWithTag<Text>("text1");
			var text2 = view.GetViewWithTag<Text>("text2");

			view.Frame = new Rect(0, 0, 320, 600);
			view.Measure(view.Frame.Width, view.Frame.Height);

			Assert.True(view.MeasurementValid);
			Assert.Equal(new Size(320, 40), view.MeasuredSize);
			Assert.Equal(new Rect(0, 0, 320, 600), view.Frame);

			Assert.True(grid.MeasurementValid);
			Assert.Equal(new Size(320, 40), grid.MeasuredSize);
			Assert.Equal(new Rect(0, 280, 320, 40), grid.Frame);

			Assert.True(textField.MeasurementValid);
			Assert.Equal(new Size(40, 12), textField.MeasuredSize);
			Assert.Equal(new Rect(0, 0, 320, 20), textField.Frame);

			Assert.True(text1.MeasurementValid);
			Assert.Equal(new Size(40, 12), text1.MeasuredSize);
			Assert.Equal(new Rect(0, 20, 160, 20), text1.Frame);

			Assert.True(text2.MeasurementValid);
			Assert.Equal(new Size(40, 12), text2.MeasuredSize);
			Assert.Equal(new Rect(160, 20, 160, 20), text2.Frame);
		}
	}
}
