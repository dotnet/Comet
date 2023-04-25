using System;
using System.Threading.Tasks;
using Comet.Tests.Handlers;
using Microsoft.Maui;
using Xunit;
namespace Comet.Tests.Cases
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
			view.SetViewHandlerToGeneric();

			(textField as ITextInput).Text = "Test";
			Assert.Equal("Test", textField.Text);
			Assert.Equal("Test", text.Value);

		}

		[Fact]
		public void StateTRequiresReadonly()
		{
			Assert.Throws<ReadonlyRequiresException>(() => {
				var view = new BadStateView();

				view.SetViewHandlerToGeneric();
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

			var viewHandler = view.SetViewHandlerToGeneric();

			var textHandler = text.SetViewHandlerToGeneric();

			Assert.Equal(startingValue, text.Value);

			const string endingValue = "Good Bye!";
			view.text.Value = endingValue;

			Assert.Equal(endingValue, text.Value);
			//Also make sure the Handler got the update
			Assert.True(textHandler.ChangedProperties.TryGetValue(nameof(IText.Text), out var changedText), "Text.Value Change was not set to Text handler");
		}

		[Fact]
		public void LabelFormattedTextBinding()
		{
			Text text = null;
			var view = new StatePage();
			const string startingValue = "Hello";

			view.text.Value = startingValue;
			view.Body = () => (text = new Text(() => view.text.Value));


			var viewHandler = view.SetViewHandlerToGeneric();

			var textHandler = text.SetViewHandlerToGeneric();

			Assert.Equal(startingValue, text.Value);

			const string endingValue = "Good Bye!";
			view.text.Value = endingValue;

			Assert.Equal(endingValue, text.Value);
			//Also make sure the Handler got the update
			Assert.True(textHandler.ChangedProperties.TryGetValue(nameof(IText.Text), out var changedText), "Text.Value Change was not set to Text handler");
		}

		[Fact]
		public void BindingShouldKeepUpdate()
		{
			Text text = null;
			Text text1 = null;
			var view = new StatePage();

			view.Body = () => new VStack
			{
				(text = new Text(() => $"{view.clickCount}")),
				(text1 = new Text(() => $"{view.clickCount}")),
			};
			view.SetViewHandlerToGeneric();

			for (int i = 1; i < 10; ++i)
			{
				Assert.Equal(text.Value.CurrentValue, $"{i}");
				Assert.Equal(text1.Value.CurrentValue, $"{i}");
				view.clickCount.Value++;
			}
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

			var viewHandler = view.SetViewHandlerToGeneric();

			var textHandler = text.SetViewHandlerToGeneric();


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

			var viewHandler = view.SetViewHandlerToGeneric();

			var textHandler = text.SetViewHandlerToGeneric();


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

			var viewHandler = view.SetViewHandlerToGeneric();

			var textHandler = text.SetViewHandlerToGeneric();


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

			var viewHandler = view.SetViewHandlerToGeneric();
			var textHandler = text.SetViewHandlerToGeneric();

			var stackHandler = stack.SetViewHandlerToGeneric();
			var textFieldHandler = textField.SetViewHandlerToGeneric();


			view.text.Value = "Good bye";

			const string tfValue = nameof(IEntry.Text);
			const string tValue = nameof(ILabel.Text);

			Assert.True(textFieldHandler.ChangedProperties.ContainsKey(tfValue), "TextField Value didnt change");

			Assert.True(textHandler.ChangedProperties.ContainsKey(tValue), "Text Value didnt change");

			var text1Value = textFieldHandler.ChangedProperties[tfValue];
			var text2Value = textHandler.ChangedProperties[tValue];
			Assert.Equal(text.Value.CurrentValue, textField.Text.CurrentValue);
		}


		[Fact]
		public void BindingWorksWithNestedChildPassedIn()
		{

			Text text = null;
			var view = new StatePage();
			const string startingValue = "0";
			var model = new ParentClassWithState();

			view.Body = () => (text = new Text(() => $"{model.CurrentDataModel.Value?.Count ?? 0}"));


			var viewHandler = view.SetViewHandlerToGeneric();
			var textHandler = text.SetViewHandlerToGeneric();

			Assert.Equal(startingValue, text.Value.CurrentValue);

			const string endingValue = "1";
			model.CurrentDataModel.Value = new MyDataModel { Count = 1 };

			model.CurrentDataModel.Value.Count = 1;
			Assert.Equal(endingValue, text.Value);

			model.CurrentDataModel.Value.Count = 2;
			Assert.Equal("2", text.Value);

		}


		[Fact]
		public async Task DatabindingWorksAcrossThreads()
		{

			bool hasFirstViewStarted = false;
			bool hasFirstViewEnded = false;

			StatePage firstView = null;
			Text firstText = null;

			int firstViewBuildCount = 0;
			int firstTextBuildCount = 0;


			bool hasSecondViewStarted = false;
			bool hasSecondViewEnded = false;

			StatePage secondView = null;
			Text secondText = null;

			int secondViewBuildCount = 0;
			int secondTextBuildCount = 0;


			var firstViewTask = Task.Run(() => {
				firstView = new StatePage();
				firstView.text.Value = "Hello";
				firstView.Body = () => {
					hasFirstViewStarted = true;
					while (!hasSecondViewStarted)
						Task.Delay(10).Wait();
					firstViewBuildCount++;
					firstText = new Text(() => {
						firstTextBuildCount++;
						return firstView.text.Value;
					});

					hasFirstViewEnded = true;
					while (!hasSecondViewEnded)
						Task.Delay(10).Wait();
					return firstText;
				};

				var viewHandler = firstView.SetViewHandlerToGeneric();

				var textHandler = firstView.SetViewHandlerToGeneric();

			});


			var secondViewTask = Task.Run(() => {
				secondView = new StatePage();
				secondView.text.Value = "Hello";
				secondView.Body = () => {
					hasSecondViewStarted = true;
					while (!hasFirstViewStarted)
						Task.Delay(10).Wait();
					secondViewBuildCount++;
					secondText = new Text(() => {
						secondTextBuildCount++;
						return secondView.text.Value;
					});

					hasSecondViewEnded = true;
					while (!hasFirstViewEnded)
						Task.Delay(10).Wait();
					return secondText;
				};

				var viewHandler = secondView.SetViewHandlerToGeneric();

				var textHandler = secondText.SetViewHandlerToGeneric();

			});


			await Task.WhenAll(firstViewTask, secondViewTask);


			Assert.Equal(1, firstViewBuildCount);
			Assert.Equal(1, firstTextBuildCount);
			Assert.Equal(1, secondViewBuildCount);
			Assert.Equal(1, secondTextBuildCount);

			firstView.text.Value = "Goodbye";

			Assert.Equal(1, firstViewBuildCount);
			Assert.Equal(2, firstTextBuildCount);
			Assert.Equal(1, secondViewBuildCount);
			Assert.Equal(1, secondTextBuildCount);

			secondView.text.Value = "Goodbye";

			Assert.Equal(1, firstViewBuildCount);
			Assert.Equal(2, firstTextBuildCount);
			Assert.Equal(1, secondViewBuildCount);
			Assert.Equal(2, secondTextBuildCount);

		}

		[Fact]
		public void TestButtonClick()
		{
			var view = new StatePage();
			view.Body = () => new VStack
			{
				new Text(() => $"{view.clickCount}").Tag("text"),
				new Button("Click", () => view.clickCount.Value ++ ).Tag("button"),
			};

			view.SetViewHandlerToGeneric();

			var button = view.GetViewWithTag<Button>("button");
			var text = view.GetViewWithTag<Text>("text");

			if (button.Clicked.Value is Action func) func();
			Assert.Equal(text.Value, "2");
		}

	}
}
