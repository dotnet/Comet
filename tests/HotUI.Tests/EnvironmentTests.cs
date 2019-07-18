using System;
using HotUI.Internal;
using HotUI.Tests.Handlers;
using Xunit;

namespace HotUI.Tests {
	public class EnvironmentTests : TestBase {

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

		public class SecondPage : View {

			[Environment]
			public readonly State<int> clickCount;

		}


		[Fact]
		public void CanSetAndReadGlobalEnvironment()
		{
            ResetHotUI();

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
            ResetHotUI();
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
            ResetHotUI();
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
            ResetHotUI();
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
            ResetHotUI();
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
            ResetHotUI();
            View.SetGlobalEnvironment (nameof (StatePage.clickCount), new State<int> (1));

			Text text = null;
			var view = new StatePage ();
			view.Body = () =>(text =  new Text (() => view.clickCount.Value.ToString ()));

			Assert.NotNull (view.clickCount);
			Assert.Equal (1, view.clickCount.Value);

			var viewHandler = new GenericViewHandler ();
			view.ViewHandler = viewHandler;


			var textHandler = new GenericViewHandler ();
			text.ViewHandler = textHandler;

			view.clickCount.Value++;
			Assert.Equal (2, view.clickCount.Value);


			Assert.Equal ("2", text.Value);


			View.SetGlobalEnvironment (nameof (StatePage.clickCount), new State<int> (3));


			Assert.Equal (3, view.clickCount.Value);


			Assert.Equal ("3", text.Value);


		}
	}
}
