using System;
using Microsoft.Maui.Graphics;
using Comet.Tests.Handlers;
using Xunit;

namespace Comet.Tests
{
	public class HStackTests : TestBase
	{
		public class HStackTestView1 : View
		{
			public readonly State<string> text = "Test";

			[Body]
			View body() => new HStack
				{
					new TextField(text).Tag("textfield"),
					new Spacer().Tag("spacer"),
					new Text(text).Tag("text")
				}.Tag("stack");
		}


		[Fact(Skip = "Needs Fixing")]
		public void TestView1()
		{
			var view = new HStackTestView1();
			InitializeHandlers(view);

			var stack = view.GetViewWithTag<HStack>("stack");
			var textField = view.GetViewWithTag<TextField>("textfield");
			var spacer = view.GetViewWithTag<Spacer>("spacer");
			var text = view.GetViewWithTag<Text>("text");

			view.Frame = new Rect(0, 0, 320, 600);

			Assert.True(view.MeasurementValid);
			Assert.Equal(new Size(320, 12), view.MeasuredSize);
			Assert.Equal(new Rect(0, 0, 320, 600), view.Frame);

			Assert.True(stack.MeasurementValid);
			Assert.Equal(new Size(320, 12), stack.MeasuredSize);
			Assert.Equal(new Rect(0, 294, 320, 12), stack.Frame);

			Assert.True(textField.MeasurementValid);
			Assert.Equal(new Size(40, 12), textField.MeasuredSize);
			Assert.Equal(new Rect(0, 0, 40, 12), textField.Frame);

			Assert.True(spacer.MeasurementValid);
			Assert.Equal(new Size(-1, -1), spacer.MeasuredSize);
			Assert.Equal(new Rect(40, 0, 240, 12), spacer.Frame);

			Assert.True(text.MeasurementValid);
			Assert.Equal(new Size(40, 12), text.MeasuredSize);
			Assert.Equal(new Rect(280, 0, 40, 12), text.Frame);
		}


		[Fact(Skip = "Needs Fixing")]
		public void TestView1WithFrameConstraints()
		{
			var view = new HStackTestView1();
			InitializeHandlers(view, 320, 600);

			var stack = view.GetViewWithTag<HStack>("stack");
			var textField = view.GetViewWithTag<TextField>("textfield");
			var spacer = view.GetViewWithTag<Spacer>("spacer");
			var text = view.GetViewWithTag<Text>("text");

			Assert.True(view.MeasurementValid);
			Assert.Equal(new Size(320, 12), view.MeasuredSize);
			Assert.Equal(new Rect(0, 0, 320, 600), view.Frame);

			Assert.True(stack.MeasurementValid);
			Assert.Equal(new Size(320, 12), stack.MeasuredSize);
			Assert.Equal(new Rect(0, 294, 320, 12), stack.Frame);

			Assert.True(textField.MeasurementValid);
			Assert.Equal(new Size(40, 12), textField.MeasuredSize);
			Assert.Equal(new Rect(0, 0, 40, 12), textField.Frame);

			Assert.True(spacer.MeasurementValid);
			Assert.Equal(new Size(-1, -1), spacer.MeasuredSize);
			Assert.Equal(new Rect(40, 0, 240, 12), spacer.Frame);

			Assert.True(text.MeasurementValid);
			Assert.Equal(new Size(40, 12), text.MeasuredSize);
			Assert.Equal(new Rect(280, 0, 40, 12), text.Frame);
		}


		[Fact(Skip = "Needs Fixing")]
		public void TestView1WithMarginOnStack()
		{
			var view = new HStackTestView1();
			InitializeHandlers(view);

			var stack = view.GetViewWithTag<HStack>("stack").Margin();
			var textField = view.GetViewWithTag<TextField>("textfield");
			var spacer = view.GetViewWithTag<Spacer>("spacer");
			var text = view.GetViewWithTag<Text>("text");

			view.Frame = new Rect(0, 0, 320, 600);

			Assert.True(view.MeasurementValid);
			Assert.Equal(new Size(300, 12), view.MeasuredSize);
			Assert.Equal(new Rect(0, 0, 320, 600), view.Frame);

			Assert.True(stack.MeasurementValid);
			Assert.Equal(new Size(300, 12), stack.MeasuredSize);
			Assert.Equal(new Rect(10, 294, 300, 12), stack.Frame);

			Assert.True(textField.MeasurementValid);
			Assert.Equal(new Size(40, 12), textField.MeasuredSize);
			Assert.Equal(new Rect(0, 0, 40, 12), textField.Frame);

			Assert.True(spacer.MeasurementValid);
			Assert.Equal(new Size(-1, -1), spacer.MeasuredSize);
			Assert.Equal(new Rect(40, 0, 220, 12), spacer.Frame);

			Assert.True(text.MeasurementValid);
			Assert.Equal(new Size(40, 12), text.MeasuredSize);
			Assert.Equal(new Rect(260, 0, 40, 12), text.Frame);
		}


		[Fact(Skip = "Needs Fixing")]
		public void TestView1WithMarginAndFrameConstraintsOnStack()
		{
			var view = new HStackTestView1();
			InitializeHandlers(view);

			var stack = view.GetViewWithTag<HStack>("stack").Margin().Frame(height: 20);
			var textField = view.GetViewWithTag<TextField>("textfield");
			var spacer = view.GetViewWithTag<Spacer>("spacer");
			var text = view.GetViewWithTag<Text>("text");

			view.Frame = new Rect(0, 0, 320, 600);

			Assert.True(view.MeasurementValid);
			Assert.Equal(new Size(300, 20), view.MeasuredSize);
			Assert.Equal(new Rect(0, 0, 320, 600), view.Frame);

			Assert.True(stack.MeasurementValid);
			Assert.Equal(new Size(300, 20), stack.MeasuredSize);
			Assert.Equal(new Rect(10, 290, 300, 20), stack.Frame);

			Assert.True(textField.MeasurementValid);
			Assert.Equal(new Size(40, 12), textField.MeasuredSize);
			Assert.Equal(new Rect(0, 4, 40, 12), textField.Frame);

			Assert.True(spacer.MeasurementValid);
			Assert.Equal(new Size(-1, -1), spacer.MeasuredSize);
			Assert.Equal(new Rect(40, 4, 220, 12), spacer.Frame);

			Assert.True(text.MeasurementValid);
			Assert.Equal(new Size(40, 12), text.MeasuredSize);
			Assert.Equal(new Rect(260, 4, 40, 12), text.Frame);
		}


		[Fact(Skip = "Needs Fixing")]
		public void TestView1WithMarginAndFrameConstraintsOnStackAndItems()
		{
			var view = new HStackTestView1();
			InitializeHandlers(view);

			var stack = view.GetViewWithTag<HStack>("stack").Margin().Frame(height: 20);
			var textField = view.GetViewWithTag<TextField>("textfield");//.Frame(alignment: Alignment.Top);
			var spacer = view.GetViewWithTag<Spacer>("spacer");
			var text = view.GetViewWithTag<Text>("text");//.Frame(alignment: Alignment.Bottom);

			view.Frame = new Rect(0, 0, 320, 600);

			Assert.True(view.MeasurementValid);
			Assert.Equal(new Size(300, 20), view.MeasuredSize);
			Assert.Equal(new Rect(0, 0, 320, 600), view.Frame);

			Assert.True(stack.MeasurementValid);
			Assert.Equal(new Size(300, 20), stack.MeasuredSize);
			Assert.Equal(new Rect(10, 290, 300, 20), stack.Frame);

			Assert.True(textField.MeasurementValid);
			Assert.Equal(new Size(40, 12), textField.MeasuredSize);
			Assert.Equal(new Rect(0, 0, 40, 12), textField.Frame);

			Assert.True(spacer.MeasurementValid);
			Assert.Equal(new Size(-1, -1), spacer.MeasuredSize);
			Assert.Equal(new Rect(40, 4, 220, 12), spacer.Frame);

			Assert.True(text.MeasurementValid);
			Assert.Equal(new Size(40, 12), text.MeasuredSize);
			Assert.Equal(new Rect(260, 8, 40, 12), text.Frame);
		}


		[Fact(Skip = "Needs Fixing")]
		public void TestView1WithMarginAndFrameConstraintsOnItems()
		{
			var view = new HStackTestView1();
			InitializeHandlers(view);

			var stack = view.GetViewWithTag<HStack>("stack").Margin();
			var textField = view.GetViewWithTag<TextField>("textfield").Frame(height: 22);
			var spacer = view.GetViewWithTag<Spacer>("spacer");
			var text = view.GetViewWithTag<Text>("text").Frame(height: 18);

			view.Frame = new Rect(0, 0, 320, 600);

			Assert.True(view.MeasurementValid);
			Assert.Equal(new Size(300, 22), view.MeasuredSize);
			Assert.Equal(new Rect(0, 0, 320, 600), view.Frame);

			Assert.True(stack.MeasurementValid);
			Assert.Equal(new Size(300, 22), stack.MeasuredSize);
			Assert.Equal(new Rect(10, 289, 300, 22), stack.Frame);

			Assert.True(textField.MeasurementValid);
			Assert.Equal(new Size(40, 22), textField.MeasuredSize);
			Assert.Equal(new Rect(0, 0, 40, 22), textField.Frame);

			Assert.True(spacer.MeasurementValid);
			Assert.Equal(new Size(-1, -1), spacer.MeasuredSize);
			Assert.Equal(new Rect(40, 0, 220, 22), spacer.Frame);

			Assert.True(text.MeasurementValid);
			Assert.Equal(new Size(40, 18), text.MeasuredSize);
			Assert.Equal(new Rect(260, 2, 40, 18), text.Frame);
		}


		[Fact(Skip = "Needs Fixing")]
		public void TestView1WithMarginAndFrameConstraintsAndMarginOnItems()
		{
			var view = new HStackTestView1();
			InitializeHandlers(view);

			var stack = view.GetViewWithTag<HStack>("stack").Margin();
			var textField = view.GetViewWithTag<TextField>("textfield").Frame(height: 22).Margin();
			var spacer = view.GetViewWithTag<Spacer>("spacer");
			var text = view.GetViewWithTag<Text>("text").Frame(height: 18);

			view.Frame = new Rect(0, 0, 320, 600);
			view.Measure(view.Frame.Width, view.Frame.Height);

			Assert.True(view.MeasurementValid);
			Assert.Equal(new Size(300, 42), view.MeasuredSize);
			Assert.Equal(new Rect(0, 0, 320, 600), view.Frame);

			Assert.True(stack.MeasurementValid);
			Assert.Equal(new Size(300, 42), stack.MeasuredSize);
			Assert.Equal(new Rect(10, 279, 300, 42), stack.Frame);

			Assert.True(textField.MeasurementValid);
			Assert.Equal(new Size(40, 22), textField.MeasuredSize);
			Assert.Equal(new Rect(10, 10, 40, 22), textField.Frame);

			Assert.True(spacer.MeasurementValid);
			Assert.Equal(new Size(-1, -1), spacer.MeasuredSize);
			Assert.Equal(new Rect(60, 10, 200, 22), spacer.Frame);

			Assert.True(text.MeasurementValid);
			Assert.Equal(new Size(40, 18), text.MeasuredSize);
			Assert.Equal(new Rect(260, 12, 40, 18), text.Frame);
		}

		public class HStackTestView2 : View
		{
			public readonly State<string> text = "Test";

			[Body]
			View body() => new HStack(spacing: 10)
			{
				new TextField(text).Tag("textfield"),
				new Text(text).Tag("text")
			}.Tag("stack");
		}


		[Fact(Skip = "Needs Fixing")]
		public void TestView2()
		{
			var view = new HStackTestView2();
			InitializeHandlers(view);

			var stack = view.GetViewWithTag<HStack>("stack");
			var textField = view.GetViewWithTag<TextField>("textfield");
			var text = view.GetViewWithTag<Text>("text");

			view.Frame = new Rect(0, 0, 320, 600);

			Assert.True(view.MeasurementValid);
			Assert.Equal(new Size(90, 12), view.MeasuredSize);
			Assert.Equal(new Rect(0, 0, 320, 600), view.Frame);

			Assert.True(stack.MeasurementValid);
			Assert.Equal(new Size(90, 12), stack.MeasuredSize);
			Assert.Equal(new Rect(115, 294, 90, 12), stack.Frame);

			Assert.True(textField.MeasurementValid);
			Assert.Equal(new Size(40, 12), textField.MeasuredSize);
			Assert.Equal(new Rect(0, 0, 40, 12), textField.Frame);

			Assert.True(text.MeasurementValid);
			Assert.Equal(new Size(40, 12), text.MeasuredSize);
			Assert.Equal(new Rect(50, 0, 40, 12), text.Frame);
		}
	}
}
