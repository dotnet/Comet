using System;
using Comet.Internal;
using Comet.Tests.Handlers;
using Xunit;

namespace Comet.Tests.Cases
{
	public class EnvironmentTests : TestBase
	{

		public class MyBindingObject : BindingObject
		{
			public string Foo
			{
				get => GetProperty<string>() ?? "Bar";
				set => SetProperty(value);
			}
		}
		public class StatePage : View
		{

			[Environment]
			public readonly State<int> clickCount;

			[Environment]
			public readonly MyBindingObject myBindingObject;
		}

		public class SecondPage : View
		{

			[Environment]
			public readonly State<int> clickCount;

		}


		[Fact]
		public void CanSetAndReadGlobalEnvironment()
		{
			ResetComet();

			const string myStringConstant = "myString";
			const string myStringKey = "myString";

			//Not set so should be null
			var environmentEntry = View.GetGlobalEnvironment<string>(myStringKey);

			Assert.Null(environmentEntry);

			View.SetGlobalEnvironment(myStringKey, myStringConstant);

			environmentEntry = View.GetGlobalEnvironment<string>(myStringKey);

			Assert.Equal(myStringConstant, environmentEntry);

		}


		[Fact]
		public void CanSetAndReadGlobalEnvironmentFromView()
		{
			ResetComet();
			const string myStringConstant = "myString";
			const string myStringKey = "myString";

			View.SetGlobalEnvironment(myStringKey, myStringConstant);

			var myView = new View();

			var environmentEntry = myView.GetEnvironment<string>(myStringKey);

			Assert.Equal(myStringConstant, environmentEntry);

		}

		[Fact]
		public void ViewEnvironmentOverwritesGlobal()
		{
			ResetComet();
			const string myStringConstant = "myString";
			const string myStringKey = "myString";
			const string parentStringValue = "myParentString";

			View.SetGlobalEnvironment(myStringKey, myStringConstant);

			var myView = new View().SetEnvironment(myStringKey, parentStringValue);

			var environmentEntry = myView.GetEnvironment<string>(myStringKey);

			Assert.Equal(parentStringValue, environmentEntry);

		}


		[Fact]
		public void NestedViewGetsValueFromParent()
		{
			ResetComet();
			const string myStringConstant = "myString";
			const string myStringKey = "myString";
			const string parentStringValue = "myParentString";

			View.SetGlobalEnvironment(myStringKey, myStringConstant);

			Text text = null;

			var view = new View
			{
				Body = () => (text = new Text())
			}.SetEnvironment(myStringKey, parentStringValue, cascades:true);

			var viewHandler = view.SetViewHandlerToGeneric();
			var textHandler = text.SetViewHandlerToGeneric();

			var environmentEntry = text.GetEnvironment<string>(myStringKey);
			Assert.Equal(parentStringValue, environmentEntry);
		}


		[Fact]
		public void NestedViewGetsItsVariablesFromItself()
		{
			ResetComet();
			const string myStringConstant = "myString";
			const string myStringKey = "myString";
			const string parentStringValue = "myParentString";
			const string testStringValue = "myTextString";

			View.SetGlobalEnvironment(myStringKey, myStringConstant);

			Text text1 = null;
			Text text2 = null;

			var view = new View
			{
				Body = () => new VStack {
					(text1 = new Text ().SetEnvironment(myStringKey,testStringValue)),
					(text2 = new Text ())
				}.SetEnvironment(myStringKey, parentStringValue, cascades: true)
			};


			var viewHandler = view.SetViewHandlerToGeneric();

			var text1Value = text1.GetEnvironment<string>(myStringKey);
			Assert.Equal(testStringValue, text1Value);

			var text2Value = text2.GetEnvironment<string>(myStringKey);
			Assert.Equal(parentStringValue, text2Value);
		}

		[Fact]
		public void FieldsWithAttributesPopulateFromEnvironment()
		{
			ResetComet();
			View.SetGlobalEnvironment(nameof(StatePage.clickCount), new State<int>(1));

			Text text = null;
			var view = new StatePage();
			view.Body = () => (text = new Text(() => view.clickCount.Value.ToString()));

			InitializeHandlers(view);

			Assert.NotNull(view.clickCount);
			Assert.Equal(1, view.clickCount.Value);


			var viewHandler = view.SetViewHandlerToGeneric();
			var textHandler = text.SetViewHandlerToGeneric();

			view.clickCount.Value++;
			Assert.Equal(2, view.clickCount.Value);


			Assert.Equal("2", text.Value);


			View.SetGlobalEnvironment(nameof(StatePage.clickCount), new State<int>(3));


			Assert.Equal(3, view.clickCount.Value);


			Assert.Equal("3", text.Value);

		}

		[Fact]
		public void LocalContextOverridesCascaded()
		{
			ResetComet();
			const string myStringKey = "myString";
			const string globalValue = "globalValue";
			const string localValue = "localValue";
			const string cascadedValue = "casadedValue";
			const string secondCascadedValue = "secondCascadedValue";

			View.SetGlobalEnvironment(myStringKey, globalValue);

			VStack rootStack = null;
			VStack stack1 = null;
			VStack stack2 = null;
			Text text1 = null;
			Text text2 = null;

			var view = new View
			{
				Body = () => rootStack = new VStack {
					(stack1 = new VStack {
						(text1 = new Text())
					}).SetEnvironment(myStringKey, cascadedValue,cascades:true),
					(stack2 = new VStack {
						(text2 = new Text()).SetEnvironment(myStringKey, secondCascadedValue,cascades:true),
					})
				}.SetEnvironment(myStringKey, localValue, cascades: false)
			};


			var viewHandler = view.SetViewHandlerToGeneric();


			void CheckView(View v, string expectedValue)
			{
				var value = v.GetEnvironment<string>(myStringKey);
				Assert.Equal(expectedValue, value);
			}

			CheckView(stack2, globalValue);
			CheckView(rootStack, localValue);
			CheckView(stack1, cascadedValue);
			CheckView(text1, cascadedValue);
			CheckView(text2, secondCascadedValue);

		}
	}
}
