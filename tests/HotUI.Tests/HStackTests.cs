using System;
using System.Drawing;
using HotUI.Tests.Handlers;
using Xunit;

namespace HotUI.Tests 
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

		[Fact]
		public void TestView1()
		{
			var view = new HStackTestView1();
			InitializeHandlers(view);

			var stack = view.GetViewWithTag<HStack>("stack");
			var textField = view.GetViewWithTag<TextField>("textfield");
			var spacer = view.GetViewWithTag<Spacer>("spacer");
			var text = view.GetViewWithTag<Text>("text");

			view.Frame = new RectangleF(0,0,320,600);
			
			Assert.Equal(true, view.MeasurementValid);
			Assert.Equal(new SizeF(320, 12), view.MeasuredSize);
			Assert.Equal(new RectangleF(0,0,320,600), view.Frame);

			Assert.Equal(true, stack.MeasurementValid);
			Assert.Equal(new SizeF(320, 12), stack.MeasuredSize);
			Assert.Equal(new RectangleF(0,294,320,12), stack.Frame);
			
			Assert.Equal(true,textField.MeasurementValid);
			Assert.Equal(new SizeF(40, 12), textField.MeasuredSize);
			Assert.Equal(new RectangleF(0,0,40,12), textField.Frame);

			Assert.Equal(true,spacer.MeasurementValid);
			Assert.Equal(new SizeF(240, 12), spacer.MeasuredSize);
			Assert.Equal(new RectangleF(40,0,240,12), spacer.Frame);

			Assert.Equal(true,text.MeasurementValid);
			Assert.Equal(new SizeF(40, 12), text.MeasuredSize);
			Assert.Equal(new RectangleF(280,0,40,12), text.Frame);
		}
		
		[Fact]
		public void TestView1WithFrameConstraints()
		{
			var view = new HStackTestView1().Frame(320, 600);
			InitializeHandlers(view);

			var stack = view.GetViewWithTag<HStack>("stack");
			var textField = view.GetViewWithTag<TextField>("textfield");
			var spacer = view.GetViewWithTag<Spacer>("spacer");
			var text = view.GetViewWithTag<Text>("text");
			
			view.Frame = new RectangleF(0,0,320,600);
			
			Assert.Equal(true, view.MeasurementValid);
			Assert.Equal(new SizeF(320, 12), view.MeasuredSize);
			Assert.Equal(new RectangleF(0,0,320,600), view.Frame);

			Assert.Equal(true, stack.MeasurementValid);
			Assert.Equal(new SizeF(320, 12), stack.MeasuredSize);
			Assert.Equal(new RectangleF(0,294,320,12), stack.Frame);
			
			Assert.Equal(true,textField.MeasurementValid);
			Assert.Equal(new SizeF(40, 12), textField.MeasuredSize);
			Assert.Equal(new RectangleF(0,0,40,12), textField.Frame);

			Assert.Equal(true,spacer.MeasurementValid);
			Assert.Equal(new SizeF(240, 12), spacer.MeasuredSize);
			Assert.Equal(new RectangleF(40,0,240,12), spacer.Frame);

			Assert.Equal(true,text.MeasurementValid);
			Assert.Equal(new SizeF(40, 12), text.MeasuredSize);
			Assert.Equal(new RectangleF(280,0,40,12), text.Frame);
		}
		
		[Fact]
		public void TestView1WithPaddingOnStack()
		{
			var view = new HStackTestView1();
			InitializeHandlers(view);

			var stack = view.GetViewWithTag<HStack>("stack").Padding();
			var textField = view.GetViewWithTag<TextField>("textfield");
			var spacer = view.GetViewWithTag<Spacer>("spacer");
			var text = view.GetViewWithTag<Text>("text");
			
			view.Frame = new RectangleF(0,0,320,600);
			
			Assert.Equal(true, view.MeasurementValid);
			Assert.Equal(new SizeF(300, 12), view.MeasuredSize);
			Assert.Equal(new RectangleF(0,0,320,600), view.Frame);

			Assert.Equal(true, stack.MeasurementValid);
			Assert.Equal(new SizeF(300, 12), stack.MeasuredSize);
			Assert.Equal(new RectangleF(10,294,300,12), stack.Frame);
			
			Assert.Equal(true,textField.MeasurementValid);
			Assert.Equal(new SizeF(40, 12), textField.MeasuredSize);
			Assert.Equal(new RectangleF(0,0,40,12), textField.Frame);

			Assert.Equal(true,spacer.MeasurementValid);
			Assert.Equal(new SizeF(220, 12), spacer.MeasuredSize);
			Assert.Equal(new RectangleF(40,0,220,12), spacer.Frame);

			Assert.Equal(true,text.MeasurementValid);
			Assert.Equal(new SizeF(40, 12), text.MeasuredSize);
			Assert.Equal(new RectangleF(260,0,40,12), text.Frame);
		}
		
		[Fact]
		public void TestView1WithPaddingAndFrameConstraintsOnStack()
		{
			var view = new HStackTestView1();
			InitializeHandlers(view);

			var stack = view.GetViewWithTag<HStack>("stack").Padding().Frame(height:20);
			var textField = view.GetViewWithTag<TextField>("textfield");
			var spacer = view.GetViewWithTag<Spacer>("spacer");
			var text = view.GetViewWithTag<Text>("text");
			
			view.Frame = new RectangleF(0,0,320,600);
			
			Assert.Equal(true, view.MeasurementValid);
			Assert.Equal(new SizeF(300, 20), view.MeasuredSize);
			Assert.Equal(new RectangleF(0,0,320,600), view.Frame);

			Assert.Equal(true, stack.MeasurementValid);
			Assert.Equal(new SizeF(300, 20), stack.MeasuredSize);
			Assert.Equal(new RectangleF(10,290,300,20), stack.Frame);
			
			Assert.Equal(true,textField.MeasurementValid);
			Assert.Equal(new SizeF(40, 12), textField.MeasuredSize);
			Assert.Equal(new RectangleF(0,4,40,12), textField.Frame);

			Assert.Equal(true,spacer.MeasurementValid);
			Assert.Equal(new SizeF(220, 12), spacer.MeasuredSize);
			Assert.Equal(new RectangleF(40,4,220,12), spacer.Frame);

			Assert.Equal(true,text.MeasurementValid);
			Assert.Equal(new SizeF(40, 12), text.MeasuredSize);
			Assert.Equal(new RectangleF(260,4,40,12), text.Frame);
		}
		
		[Fact]
		public void TestView1WithPaddingAndFrameConstraintsOnStackAndItems()
		{
			var view = new HStackTestView1();
			InitializeHandlers(view);

			var stack = view.GetViewWithTag<HStack>("stack").Padding().Frame(height:20);
			var textField = view.GetViewWithTag<TextField>("textfield").Frame(alignment:Alignment.Top);
			var spacer = view.GetViewWithTag<Spacer>("spacer");
			var text = view.GetViewWithTag<Text>("text").Frame(alignment:Alignment.Bottom);
			
			view.Frame = new RectangleF(0,0,320,600);
			
			Assert.Equal(true, view.MeasurementValid);
			Assert.Equal(new SizeF(300, 20), view.MeasuredSize);
			Assert.Equal(new RectangleF(0,0,320,600), view.Frame);

			Assert.Equal(true, stack.MeasurementValid);
			Assert.Equal(new SizeF(300, 20), stack.MeasuredSize);
			Assert.Equal(new RectangleF(10,290,300,20), stack.Frame);
			
			Assert.Equal(true,textField.MeasurementValid);
			Assert.Equal(new SizeF(40, 12), textField.MeasuredSize);
			Assert.Equal(new RectangleF(0,0,40,12), textField.Frame);

			Assert.Equal(true,spacer.MeasurementValid);
			Assert.Equal(new SizeF(220, 12), spacer.MeasuredSize);
			Assert.Equal(new RectangleF(40,4,220,12), spacer.Frame);

			Assert.Equal(true,text.MeasurementValid);
			Assert.Equal(new SizeF(40, 12), text.MeasuredSize);
			Assert.Equal(new RectangleF(260,8,40,12), text.Frame);
		}
	}
}
