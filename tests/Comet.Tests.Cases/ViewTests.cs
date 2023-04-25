using System;
using Comet.Tests.Handlers;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace Comet.Tests.Cases
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


			var viewHandler = view.SetViewHandlerToGeneric();

			text = currentText;

			var textHandler = text.SetViewHandlerToGeneric();

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
		[Fact]
		public void ViewsHaveMatchingTypeHashCodes()
		{
			var text = new Text("Foo").GetContentTypeHashCode();
			var text2 = new Text("Bar").GetContentTypeHashCode();
			Assert.Equal(text, text2);

			text = new HStack()
			{
				new Text("Foo"),
			}.GetContentTypeHashCode();

			text2 = new HStack()
			{
				new Text("Bar"),
			}.GetContentTypeHashCode();

			Assert.Equal(text, text2);


			text = new HStack()
			{
				new Text("Foo"),
				new Button("Foo"),
			}.GetContentTypeHashCode();

			text2 = new HStack()
			{
				new Text("Bar"),
				new Button("Bar"),
			}.GetContentTypeHashCode();

			Assert.Equal(text, text2);

			//Not Equal Section

			text = new HStack()
			{
				new Text("Foo"),
			}.GetContentTypeHashCode();

			text2 = new HStack()
			{
				new Button("Bar"),
			}.GetContentTypeHashCode();

			Assert.NotEqual(text, text2);

			text = new HStack()
			{
				new Text("Foo"),
			}.GetContentTypeHashCode();

			text2 = new VStack()
			{
				new Button("Foo"),
			}.GetContentTypeHashCode();

			Assert.NotEqual(text, text2);
		}
	}
}
