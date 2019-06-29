using System;
using Xunit;

namespace HotUI.Tests {
	public class EnvironmentTests {

		public class MyBindingObject : BindingObject {
			public string Foo {
				get => GetProperty<string> () ?? "Bar";
				set => SetProperty (value);
			}
		}
		public class StatePage : View {

			[Environment]
			public readonly State<int> clickCount;

			[Environment]
			public readonly MyBindingObject myBindingObject;
		}


		[Fact]
		public void CanSetAndReadGlobalEnvironment()
		{
			View.Environment.dictionary.Clear ();

			const string myStringConstant = "myString";
			const string myStringKey = "myString";

			//Not set so should be null
			var environmentEntry = View.GetGlobalEnvironment<string> (myStringKey);

			Assert.Null (environmentEntry);

			View.SetGlobalEnvironment (myStringKey, myStringConstant);

			environmentEntry = View.GetGlobalEnvironment<string> (myStringKey);

			Assert.Equal (myStringConstant, environmentEntry);

		}


		[Fact]
		public void CanSetAndReadGlobalEnvironmentFromView ()
		{
			const string myStringConstant = "myString";
			const string myStringKey = "myString";

			View.SetGlobalEnvironment (myStringKey, myStringConstant);

			var myView = new View ();
		
			var environmentEntry = myView.GetEnvironment<string> (myStringKey);

			Assert.Equal (myStringConstant, environmentEntry);

		}

		[Fact]
		public void ViewEnvironmentOverwritesGlobal ()
		{
			const string myStringConstant = "myString";
			const string myStringKey = "myString";
			const string parentStringValue = "myParentString";

			View.SetGlobalEnvironment (myStringKey, myStringConstant);

			var myView = new View ().SetEnvironment(myStringKey,parentStringValue);

			var environmentEntry = myView.GetEnvironment<string> (myStringKey);

			Assert.Equal (parentStringValue, environmentEntry);

		}


		[Fact]
		public void NestedViewGetsValueFromParent ()
		{

			const string myStringConstant = "myString";
			const string myStringKey = "myString";
			const string parentStringValue = "myParentString";

			View.SetGlobalEnvironment (myStringKey, myStringConstant);

			Text text = null;

			var view = new View {
				Body = () => (text = new Text ())
			}.SetEnvironment(myStringKey, parentStringValue);

			var viewHandler = new GenericViewHandler ();
			view.ViewHandler = viewHandler;

			var textHandler = new GenericViewHandler ();
			text.ViewHandler = textHandler;

			var environmentEntry = text.GetEnvironment<string> (myStringKey);
			Assert.Equal (parentStringValue, environmentEntry);
		}


		[Fact]
		public void NestedViewGetsItsVariablesFromItself ()
		{

			const string myStringConstant = "myString";
			const string myStringKey = "myString";
			const string parentStringValue = "myParentString";
			const string testStringValue = "myTextString";

			View.SetGlobalEnvironment (myStringKey, myStringConstant);

			Text text1 = null;
			Text text2 = null;

			var view = new View {
				Body = () => new VStack {
					(text1 = new Text ().SetEnvironment(myStringKey,testStringValue)),
					(text2 = new Text ())
				}.SetEnvironment (myStringKey, parentStringValue)
			};

			var viewHandler = new GenericViewHandler ();
			view.ViewHandler = viewHandler;

			var text1Value = text1.GetEnvironment<string> (myStringKey);
			Assert.Equal (testStringValue, text1Value);

			var text2Value = text2.GetEnvironment<string> (myStringKey);
			Assert.Equal (parentStringValue, text2Value);
		}

		[Fact]
		public void FieldsWithAttributesPopulateFromEnvironment ()
		{

			View.SetGlobalEnvironment (nameof (StatePage.clickCount), new State<int> (1));

			var page = new StatePage ();
			page.Body = () => new Text (() => page.clickCount.Value.ToString ());

			Assert.NotNull (page.clickCount);
			Assert.Equal (1, page.clickCount.Value);
		}
	}
}
