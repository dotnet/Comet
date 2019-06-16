using System;
using System.Linq;

namespace HotForms.Sample {
	public class MainPage : StateHotPage {
		protected override void CreateState (dynamic state)
		{
			state.Foo = "Hello";
			state.ClickCount = 1;
			state.ImageUrl = "http://lh3.googleusercontent.com/_0mvh5XaOwhfDROmqadX1N7morS0kYH7Za4Ym9C5W8H0vQq9X4OkAJz-NTDgOX_Sq1ZIfQi-Vw";
			state.Url = "https://www.Xamarin.com";
		}

		protected override Xamarin.Forms.View Build (dynamic state) => new Stack {
			new Image {
				Source = state.ImageUrl,
			},
			new Label {
				Text = state.Foo
			},
			new Button {
				Text = "Click Me",
				OnClick =()=>{
					state.Foo = $"Clicked: {state.ClickCount++}";
				},
			},
			//new WebView {
			//	VerticalOptions = LayoutOptions.FillAndExpand,
			//	Source = state.Url,
			//}
			new ListView {
				new Stack {
					new Label {
						Text = "Foo",
					}
				},
				new Xamarin.Forms.Label {
					Text = "Foo",
				},
			}
		};
	}

	public interface IFoo : IState {
		string Foo { get; set; }
		int ClickCount { get; set; }
	}
	public class MainPage2 : StateHotPage<IFoo> {
		protected override void CreateState (IFoo state)
		{
			state.Foo = "Hello";
			state.ClickCount = 1;
		}

		protected override Xamarin.Forms.View Build (IFoo state) => new Stack {
			new Label {
				Text = state.Foo
			},
			new Button {
				Text = "Click Me",
				OnClick =()=>{
					state.Foo = $"Clicked: {state.ClickCount++}";
				},
			}
		};
	}

	/// <summary>
	/// This one lets you pass in any arbitrary list of view or cells. Great for a settings screen
	/// </summary>
	public class ListPage : HotPage {
		protected override Xamarin.Forms.View Build () => new ListView {
			new Stack {
				new Label("First Item"),
			},
			new Stack {
				new Label("Second Item"),
			},
			new Xamarin.Forms.SwitchCell(),
		};
	}


	public class ListPage1 : HotPage {
		protected override Xamarin.Forms.View Build () => new ListView {
			ItemsSource = Enumerable.Range(0,10),
			ViewFor  = (x) => new Stack {
				new Label(x.ToString()),
				new Label("Hi"),
			},
		};
	}



	public class ListPage2 : HotPage {
		class MyDataModel {
			public string Foo { get; set; } = "Foo";
			public string Bar { get; set; } = "Bar";
			public int Index { get; set; }
		}
		protected override Xamarin.Forms.View Build () => new ListView<MyDataModel> {
			ItemsSource = Enumerable.Range (0, 10).Select(x=> new MyDataModel { Index = x }),
			ViewFor = (x) => new Stack {
				new Label($"Index: {x.Index}"),
				new Label($"Foo: {x.Foo}"),
				new Label($"Bar: {x.Bar}"),
			},
		};
	}

}
