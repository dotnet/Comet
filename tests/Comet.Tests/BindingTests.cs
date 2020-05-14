using System;
using Comet.Tests.Handlers;
using Xunit;
namespace Comet.Tests
{
	public class BindingTests : TestBase
	{

		public class StatePage : View
		{
			public readonly State<int> clickCount = new State<int>(1);
			public readonly State<string> text = new State<string>();
			public readonly State<bool> boolState = new State<bool>();
		}

		public class BadStateView : View
		{
			public State<int> badState = 1;
			[Body]
			View body() => new Text(() => $"badState: {badState}");
		}

		public class MyDataModel : BindingObject
		{
			public int Count
			{
				get => GetProperty<int>();
				set => SetProperty(value);
			}
			public bool BoolValue
			{
				get => GetProperty<bool>();
				set => SetProperty(value);
			}
		}

		public class ParentClassWithState
		{
			public State<MyDataModel> CurrentDataModel { get; set; } = new State<MyDataModel>();
		}

		[Fact]
		public void MagicDatabinding()
		{
			var view = new StatePage();
			TextField textField = null;
			Text text = null;
			view.Body = () => new VStack
			{
				(textField = new TextField(view.text)),
				(text = new Text(view.text)),
			};
			view.ViewHandler = new GenericViewHandler();

			textField.OnEditingChanged("Test");
			Assert.Equal("Test", textField.Text);
			Assert.Equal("Test", text.Value);

		}

		[Fact]
		public void StateTRequiresReadonly()
		{
			Assert.Throws<ReadonlyRequiresException>(() => {
				var view = new BadStateView();

				var viewHandler = new GenericViewHandler();
				view.ViewHandler = viewHandler;
			});
		}

		[Fact]
		public void LabelTextDirectBinding()
		{

			Text text = null;
			var view = new StatePage();
			const string startingValue = "Hello";

			view.text.Value = startingValue;

			view.Body = () => (text = new Text(view.text.Value));

			var viewHandler = new GenericViewHandler();
			view.ViewHandler = viewHandler;

			var textHandler = new GenericViewHandler();
			text.ViewHandler = textHandler;

			Assert.Equal(startingValue, text.Value);

			const string endingValue = "Good Bye!";
			view.text.Value = endingValue;

			Assert.Equal(endingValue, text.Value);
			//Also make sure the Handler got the update
			Assert.True(textHandler.ChangedProperties.TryGetValue(nameof(Text.Value), out var changedText), "Text.Value Change was not set to Text handler");
			Assert.Equal(endingValue, changedText);
		}

		[Fact]
		public void LabelFormattedTextBinding()
		{
			Text text = null;
			var view = new StatePage();
			const string startingValue = "Hello";

			view.text.Value = startingValue;
			view.Body = () => (text = new Text(() => view.text.Value));

			var viewHandler = new GenericViewHandler();
			view.ViewHandler = viewHandler;

			var textHandler = new GenericViewHandler();
			text.ViewHandler = textHandler;

			Assert.Equal(startingValue, text.Value);

			const string endingValue = "Good Bye!";
			view.text.Value = endingValue;

			Assert.Equal(endingValue, text.Value);
			//Also make sure the Handler got the update
			Assert.True(textHandler.ChangedProperties.TryGetValue(nameof(Text.Value), out var changedText), "Text.Value Change was not set to Text handler");
			Assert.Equal(endingValue, changedText);
		}


		[Fact]
		public void SingleBindingNotEffectedByGlobal()
		{
			Text text = null;
			var view = new StatePage();

			view.text.Value = "Hello";
			int buildCount = 0;
			int textBuildCount = 0;
			view.Body = () => {
				buildCount++;
				text = new Text(() => {
					textBuildCount++;
					return view.text.Value;
				});
				return text;
			};

			var viewHandler = new GenericViewHandler();
			view.ViewHandler = viewHandler;


			var textHandler = new GenericViewHandler();
			text.ViewHandler = textHandler;


			Assert.Equal(1, buildCount);
			Assert.Equal(1, textBuildCount);

			view.text.Value = "Goodbye";


			Assert.Equal(1, buildCount);
			Assert.Equal(2, textBuildCount);

		}


		[Fact]
		public void DoubleBindingNotEffectedByGlobal()
		{
			Text text = null;
			var view = new StatePage();

			view.text.Value = "Hello";
			int buildCount = 0;
			int textBuildCount = 0;
			view.Body = () => {
				buildCount++;
				text = new Text(() => {
					textBuildCount++;
					return $"{view.text.Value} - {view.clickCount.Value}";
				});
				return text;
			};

			var viewHandler = new GenericViewHandler();
			view.ViewHandler = viewHandler;


			var textHandler = new GenericViewHandler();
			text.ViewHandler = textHandler;


			Assert.Equal(1, buildCount);
			Assert.Equal(1, textBuildCount);

			view.clickCount.Value++;


			Assert.Equal(1, buildCount);
			Assert.Equal(2, textBuildCount);

			view.text.Value = "Good bye";


			Assert.Equal(1, buildCount);
			Assert.Equal(3, textBuildCount);

		}


		[Fact]
		public void FormatingStringInTextCausesGlobal()
		{
			Text text = null;
			var view = new StatePage();

			view.text.Value = "Hello";
			int buildCount = 0;

			view.Body = () => {
				buildCount++;
				text = new Text($"{view.text.Value} - {view.clickCount.Value}");
				return text;
			};

			var viewHandler = new GenericViewHandler();
			view.ViewHandler = viewHandler;


			var textHandler = new GenericViewHandler();
			text.ViewHandler = textHandler;


			Assert.Equal(1, buildCount);

			view.clickCount.Value++;


			Assert.Equal(2, buildCount);

			view.text.Value = "Good bye";


			Assert.Equal(3, buildCount);

		}



		[Fact]
		public void BindingMultipleLabelsUpdatesBoth()
		{
			TextField textField = null;
			Text text = null;
			VStack stack = null;
			var view = new StatePage();

			view.text.Value = "Hello";
			int buildCount = 0;

			view.Body = () => {
				buildCount++;
				textField = new TextField(view.text, "Placeholder");
				text = new Text(view.text.Value);
				stack = new VStack {
					textField,
					text

				};
				//text = new Text ($"{view.text.Value} - {view.clickCount.Value}");
				return stack;
			};

			var viewHandler = new GenericViewHandler();
			view.ViewHandler = viewHandler;

			var stackHandler = new GenericViewHandler();
			stack.ViewHandler = stackHandler;


			var textFieldHandler = new GenericViewHandler();
			textField.ViewHandler = textFieldHandler;


			var textHandler = new GenericViewHandler();
			text.ViewHandler = textHandler;

			view.text.Value = "Good bye";

			const string tfValue = nameof(TextField.Text);
			const string tValue = nameof(Text.Value);

			Assert.True(textFieldHandler.ChangedProperties.ContainsKey(tfValue), "TextField Value didnt change");

			Assert.True(textHandler.ChangedProperties.ContainsKey(tValue), "Text Value didnt change");

			var text1Value = textFieldHandler.ChangedProperties[tfValue];
			var text2Value = textHandler.ChangedProperties[tValue];
			Assert.Equal(text1Value, text2Value);
		}


		[Fact]
		public void BindingWorksWithNestedChildPassedIn()
		{

			Text text = null;
			var view = new StatePage();
			const string startingValue = "0";
			var model = new ParentClassWithState();
		
			view.Body = () => (text = new Text(()=> $"{model.CurrentDataModel.Value?.Count ?? 0}"));

			var viewHandler = new GenericViewHandler();
			view.ViewHandler = viewHandler;

			var textHandler = new GenericViewHandler();
			text.ViewHandler = textHandler;

			Assert.Equal(startingValue, text.Value.CurrentValue);

			const string endingValue = "1";
			model.CurrentDataModel.Value = new MyDataModel { Count = 1 };

			model.CurrentDataModel.Value.Count = 1;
			Assert.Equal(endingValue, text.Value);

			model.CurrentDataModel.Value.Count = 2;
			Assert.Equal("2", text.Value);

		}


	}
}
