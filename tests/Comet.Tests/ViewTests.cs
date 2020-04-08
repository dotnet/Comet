using System;
using System.Maui.Tests.Handlers;
using Xunit;

namespace System.Maui.Tests
{
	public class ViewTests : TestBase
	{

		public class StatePage : View
		{
			public readonly State<int> clickCount = new State<int>(1);
			public readonly State<string> text = new State<string>();
			public readonly State<bool> boolState = new State<bool>();
		}

		[Fact]
		public void RebuildingViewsReuseNativeView()
		{
			Text text = null;
			Text currentText = null;
			var view = new StatePage();

			view.text.Value = "Hello";
			int buildCount = 0;

			view.Body = () => {
				buildCount++;
				currentText = new Text($"{view.text.Value} - {view.clickCount.Value}");
				return currentText;
			};

			var viewHandler = new GenericViewHandler();
			view.ViewHandler = viewHandler;


			text = currentText;
			var textHandler = new GenericViewHandler();
			text.ViewHandler = textHandler;


			Assert.Equal(1, buildCount);

			view.clickCount.Value++;


			Assert.Equal(2, buildCount);


			//Check to make sure that the view was rebuilt
			Assert.NotEqual(text, currentText);
			//Make sure the old one is unasociated 
			Assert.Null(text?.ViewHandler);
			//Make sure the new view has the old handler
			Assert.Equal(textHandler, currentText?.ViewHandler);
		}
	}
}
