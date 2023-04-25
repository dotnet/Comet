using System;
using System.Collections.Generic;
using System.Text;
using Comet;
using Xunit;

namespace Comet.Tests.Cases
{
	public partial class StringConcatanator
	{
		[AutoNotify]
		string value1;

		[AutoNotify]
		string value2;

		public string Total => $"{Value1}{Value2}";
	}

	public class AutoNotifyTest : TestBase
	{

		public class MainPage : View
		{

			[State]
			public readonly StringConcatanator model = new();
			public Text TotalText { get; set; }
			[Body]
			View body()
				=> new VStack
				{
					(TotalText = new Text(model.Total))
				};

		
		}


		[Fact]
		public void VerifyAutoGeneratesAndUpdates()
		{
			var view = new MainPage();		
			view.SetViewHandlerToGeneric();
			view.model.Value1 = "Foo";
			view.model.Value2 = "Bar";

			Assert.Equal("FooBar", view.TotalText.Value);

		}

	}
}
