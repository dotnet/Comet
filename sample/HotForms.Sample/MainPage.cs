using System;

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
			new WebView {
				VerticalOptions = LayoutOptions.FillAndExpand,
				Source = state.Url,
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
}
