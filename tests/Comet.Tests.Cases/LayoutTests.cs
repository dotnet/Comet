using System;
using Microsoft.Maui.Graphics;
using Comet.Tests.Handlers;
using Xunit;
using Microsoft.Maui;
using Microsoft.Maui.Primitives;

namespace Comet.Tests.Cases
{
	public class LayoutTests : TestBase
	{
		public class TextInVStackTestView : View
		{
			public readonly State<string> text = "Test";

			[Body]
			View body() => new VStack()
			{
				new Text(text).FitHorizontal().Tag("text"),
			}.FillHorizontal()
			.FitVertical()
			.Padding(new Thickness())
			.Tag("stack");
		}

		[Fact]
		public void TextInVStackIsFullWidth()
		{
			var view = new TextInVStackTestView();
			InitializeHandlers(view, 320, 600);

			var stack = view.GetViewWithTag("stack");
			var text = view.GetViewWithTag("text");

			Assert.Equal(new Rect(0, 0, 320, 600), view.Frame);
			Assert.Equal(new Rect(0, 0, 320, 12), stack.Frame);
			Assert.Equal(new Rect(0, 0, 40, 12), text.Frame);
		}

		public class TextFieldInVStackTestView : View
		{
			public readonly State<string> text = "Test";

			[Body]
			View body() => new VStack()
			{
				new TextField(text).FillHorizontal().Tag("textfield"),
			}.FillHorizontal()
			.FitVertical()
			.Padding(new Thickness())
			.Tag("stack");
		}

		[Fact]
		public void TextFieldInVStackIsFullWidth()
		{
			var view = new TextFieldInVStackTestView();
			InitializeHandlers(view, 320, 600);

			var stack = view.GetViewWithTag("stack");
			var textfield = view.GetViewWithTag("textfield");

			Assert.Equal(new Rect(0, 0, 320, 600), view.Frame);
			Assert.Equal(new Rect(0, 0, 320, 12), stack.Frame);
			Assert.Equal(new Rect(0, 0, 320, 12), textfield.Frame);
		}

		public class SecureFieldInVStackTestView : View
		{
			public readonly State<string> text = "Test";

			[Body]
			View body() => new VStack()
			{
				new SecureField(text).Tag("securefield"),
			}.FillHorizontal()
			.FitVertical()
			.Padding(new Thickness())
			.Tag("stack");
		}

		[Fact]
		public void SecureFieldInVStackIsFullWidth()
		{
			var view = new SecureFieldInVStackTestView();
			InitializeHandlers(view, 320, 600);

			var stack = view.GetViewWithTag("stack");
			var securefield = view.GetViewWithTag("securefield");

			Assert.Equal(new Rect(0, 0, 320, 600), view.Frame);
			Assert.Equal(new Rect(0, 0, 320, 12), stack.Frame);
			Assert.Equal(new Rect(0, 0, 320, 12), securefield.Frame);
		}

		public class SliderInVStackTestView : View
		{
			[Body]
			View body() => new VStack()
			{
				new Slider(0).Tag("slider"),
			}.FillHorizontal()
			.FitVertical()
			.Padding(new Thickness())
			.Tag("stack");
		}

		[Fact]
		public void SliderInVStackIsFullWidth()
		{
			var view = new SliderInVStackTestView();
			InitializeHandlers(view, 320, 600);

			var stack = view.GetViewWithTag("stack");
			var slider = view.GetViewWithTag("slider");

			Assert.Equal(new Rect(0, 0, 320, 600), view.Frame);
			Assert.Equal(new Rect(0, 0, 320, 20), stack.Frame);
			Assert.Equal(new Rect(0, 0, 320, 20), slider.Frame);
		}

		public class ProgressBarInVStackTestView : View
		{
			[Body]
			View body() => new VStack()
			{
				new ProgressBar(0).Tag("progressbar"),
			}.FillHorizontal()
			.FitVertical()
			.Padding(new Thickness())
			.Tag("stack");
		}

		[Fact]
		public void ProgressBarInVStackIsFullWidth()
		{
			var view = new ProgressBarInVStackTestView();
			InitializeHandlers(view, 320, 600);

			var stack = view.GetViewWithTag("stack");
			var progressbar = view.GetViewWithTag("progressbar");

			Assert.Equal(new Rect(0, 0, 320, 600), view.Frame);
			Assert.Equal(new Rect(0, 0, 320, 20), stack.Frame);
			Assert.Equal(new Rect(0, 0, 320, 20), progressbar.Frame);
		}

		[Fact]
		public void ComboBoxInVStackDefaultsToAMinimumWidth()
		{

		}

		[Fact]
		public void ButtonInVStackDefaultsToAMinimumWidth()
		{

		}

		[Fact]
		public void LabelInHStackIsCenterAlignedVertically()
		{

		}

		[Fact]
		public void ListViewInAVStackTakesRemainingHeight()
		{

		}
	}
}
