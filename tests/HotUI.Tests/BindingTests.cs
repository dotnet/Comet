using System;
using Xunit;

namespace HotUI.Tests {
	public class BindingTests {

		public class StatePage : View {
			public readonly State<int> clickCount = new State<int> (1);
			public readonly State<string> text = new State<string> ();
		}

		[Fact]
		public void LabelBindingWorks ()
		{
			Text text = null;
			var view = new StatePage ();
			const string startingValue = "Hello";

			view.text.Value = startingValue;
			view.Body = () => (text = new Text (view.text.Value));

			var viewHandler = new GenericViewHandler ();
			view.ViewHandler = viewHandler;

			var textHandler = new GenericViewHandler ();
			text.ViewHandler = textHandler;

			Assert.Equal (startingValue, text.Value);

			const string endingValue = "Good Bye!";
			view.text.Value = endingValue;

			Assert.Equal (endingValue, text.Value);
			//Also make sure the Handler got the update
			Assert.True (textHandler.ChangedProperties.TryGetValue (nameof (Text.Value), out var changedText), "Text.Value Change was not set to Text handler");
			Assert.Equal (endingValue, changedText);
		}
	}
}
