using System;
using Xunit;

namespace HotUI.Tests {
	public class BindingTests {

		public class StatePage : View {
			public readonly State<int> clickCount = new State<int> (1);
			public readonly State<string> text = new State<string> ();
			public readonly State<bool> boolState = new State<bool> ();
		}

		[Fact]
		public void LabelTextDirectBinding ()
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

		[Fact]
		public void LabelFormatedTextBinding ()
		{
			Text text = null;
			var view = new StatePage ();
			const string startingValue = "Hello";

			view.text.Value = startingValue;
			view.Body = () => (text = new Text (() => view.text.Value));

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


		[Fact]
		public void SingleBindingNotEffectedByGlobal ()
		{
			Text text = null;
			var view = new StatePage ();

			view.text.Value = "Hello";
			int buildCount = 0;
			int textBuildCount = 0;
			view.Body = () => {
				buildCount++;
				text = new Text (() => {
					textBuildCount++;
					return view.text.Value;
				});
				return text;
			};

			var viewHandler = new GenericViewHandler ();
			view.ViewHandler = viewHandler;


			var textHandler = new GenericViewHandler ();
			text.ViewHandler = textHandler;


			Assert.Equal (buildCount, 1);
			Assert.Equal (textBuildCount, 1);

			view.text.Value = "Goodbye";


			Assert.Equal (buildCount, 1);
			Assert.Equal (textBuildCount, 2);

		}


		[Fact]
		public void DoubleBindingNotEffectedByGlobal ()
		{
			Text text = null;
			var view = new StatePage ();

			view.text.Value = "Hello";
			int buildCount = 0;
			int textBuildCount = 0;
			view.Body = () => {
				buildCount++;
				text = new Text (() => {
					textBuildCount++;
					return $"{view.text.Value} - {view.clickCount.Value}";
				});
				return text;
			};

			var viewHandler = new GenericViewHandler ();
			view.ViewHandler = viewHandler;


			var textHandler = new GenericViewHandler ();
			text.ViewHandler = textHandler;


			Assert.Equal (1, buildCount);
			Assert.Equal (1, textBuildCount);

			view.clickCount.Value++;


			Assert.Equal (1, buildCount);
			Assert.Equal (2, textBuildCount);

			view.text.Value = "Good bye";


			Assert.Equal (1, buildCount);
			Assert.Equal (3, textBuildCount);

		}


		[Fact]
		public void FormatingStringInTextCausesGlobal ()
		{
			Text text = null;
			var view = new StatePage ();

			view.text.Value = "Hello";
			int buildCount = 0;

			view.Body = () => {
				buildCount++;
				text = new Text ($"{view.text.Value} - {view.clickCount.Value}");
				return text;
			};

			var viewHandler = new GenericViewHandler ();
			view.ViewHandler = viewHandler;


			var textHandler = new GenericViewHandler ();
			text.ViewHandler = textHandler;


			Assert.Equal (1, buildCount);

			view.clickCount.Value++;


			Assert.Equal (2, buildCount);

			view.text.Value = "Good bye";


			Assert.Equal (3, buildCount);

		}



		[Fact]
		public void BindingMultipleLabelsUpdatesBoth ()
		{
			TextField textField = null;
			Text text = null;
			VStack stack = null;
			var view = new StatePage ();

			view.text.Value = "Hello";
			int buildCount = 0;

			view.Body = () => {
				buildCount++;
				stack = new VStack {
					(textField = new TextField (view.text.Value)),
					(text = new Text (view.text.Value),
				};
				//text = new Text ($"{view.text.Value} - {view.clickCount.Value}");
				return stack;
			};

			var viewHandler = new GenericViewHandler ();
			view.ViewHandler = viewHandler;

			var stackHandler = new GenericViewHandler ();
			stack.ViewHandler = stackHandler;


			var textFieldHandler = new GenericViewHandler ();
			textField.ViewHandler = textFieldHandler;


			var textHandler = new GenericViewHandler ();
			text.ViewHandler = textHandler;

			view.text.Value = "Good bye";

			const string tfValue = nameof (TextField.Text);
			const string tValue = nameof (Text.Value);

			Assert.True(textFieldHandler.ChangedProperties.ContainsKey (tfValue),"TextField Value didnt change");

			Assert.True (textHandler.ChangedProperties.ContainsKey (tValue),"Text Value didnt change");

			var text1Value = textFieldHandler.ChangedProperties [tfValue];
			var text2Value = textHandler.ChangedProperties [tValue];
			Assert.Equal (text1Value, text2Value);
		}



	}
}
