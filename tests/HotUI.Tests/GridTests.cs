using System;
using System.Drawing;
using HotUI.Tests.Handlers;
using Xunit;

namespace HotUI.Tests 
{
	public class GridTests : TestBase 
	{
		public class GridTestView1 : View
		{
			public readonly State<string> text = "Test";

				[Body]
				View body() => new Grid(columns: new []{"*","*"}, defaultRowHeight:20)
				{
					new TextField(text).Tag("textfield").Cell(row:0, column:0, colSpan:2),
					new Text(text).Tag("text1").Cell(row:1, column: 0),
					new Text(text).Tag("text2").Cell(row:1, column: 1),
				}.Tag("grid");
		}

		[Fact]
		public void TestView1()
		{
			var view = new GridTestView1();
			InitializeHandlers(view);

			var grid = view.GetViewWithTag<Grid>("grid");
			var textField = view.GetViewWithTag<TextField>("textfield");
			var text1 = view.GetViewWithTag<Text>("text");
			var text2 = view.GetViewWithTag<Text>("text");

			view.Frame = new RectangleF(0,0,320,600);
			
			Assert.True(view.MeasurementValid);
			Assert.Equal(new SizeF(320, 20), view.MeasuredSize);
			Assert.Equal(new RectangleF(0,0,320,600), view.Frame);

			Assert.True(grid.MeasurementValid);
			Assert.Equal(new SizeF(320, 20), grid.MeasuredSize);
			Assert.Equal(new RectangleF(0,290,320,20), grid.Frame);
			
			Assert.True(textField.MeasurementValid);
			Assert.Equal(new SizeF(40, 12), textField.MeasuredSize);
			Assert.Equal(new RectangleF(0,0,40,12), textField.Frame);

			Assert.True(text1.MeasurementValid);
			Assert.Equal(new SizeF(240, 12), text1.MeasuredSize);
			Assert.Equal(new RectangleF(40,0,240,12), text1.Frame);

			Assert.True(text2.MeasurementValid);
			Assert.Equal(new SizeF(40, 12), text2.MeasuredSize);
			Assert.Equal(new RectangleF(280,0,40,12), text2.Frame);
		}
	}
}
