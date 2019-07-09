using System;
using System.Collections.Generic;
using HotUI.Samples.Comparisons;

namespace HotUI.Samples {
	public class MainPage : View {
		List<(string Title, Func<View> View)> pages = new List<(string Title, Func<View> View)> {
			("Binding Sample   ",()=> new BindingSample()),
            ("BasicTestView",()=> new BasicTestView()),
            ("ListPage1", ()=> new ListPage()),
			("ListPage2", ()=> new ListPage2()),
            ("Insane Diff", ()=> new InsaneDiffPage()),
            ("SwiftUI Tutorial Section 1", ()=> new Section1()),
            ("SwiftUI Tutorial Section 2", ()=> new Section2()),
            ("SwiftUI Tutorial Section 3", ()=> new Section3()),
            ("SwiftUI Tutorial Section 4", ()=> new Section4()),
            ("SwiftUI Tutorial Section 4b", ()=> new Section4b()),
            ("SwiftUI Tutorial Section 4c", ()=> new Section4c()),
		};
		public MainPage ()
		{
			Body = () => new NavigationView {
				new ListView<(string Title, Func<View> View)> (pages) {
					Cell = (page) => new NavigationButton (page.Title,page.View),
				}
			};
		}

	}

	//public class MainPage : StateHotPage {
	//	protected override void CreateState (dynamic state)
	//	{
	//		state.Foo = "Hello";
	//		state.ClickCount = 1;
	//		state.ImageUrl = "http://lh3.googleusercontent.com/_0mvh5XaOwhfDROmqadX1N7morS0kYH7Za4Ym9C5W8H0vQq9X4OkAJz-NTDgOX_Sq1ZIfQi-Vw";
	//		state.Url = "https://www.Xamarin.com";
	//	}

	//	protected override View Build (dynamic state) => new Stack {
	//		new Image {
	//			Source = state.ImageUrl,
	//		},
	//		new Label {
	//			Text = state.Foo,
	//			//TextBinding = ()=> state.Foo,
	//		},
	//		new Button {
	//			Text = "Click Me",
	//			OnClick =()=>{
	//				state.Foo = $"Clicked: {state.ClickCount++}";
	//			},
	//		},
	//		//new WebView {
	//		//	VerticalOptions = LayoutOptions.FillAndExpand,
	//		//	Source = state.Url,
	//		//}
	//		new ListView {
	//			new Stack {
	//				new Label {
	//					Text = "Foo",
	//				}
	//			},
	//			new Xamarin.Forms.Label {
	//				Text = "Foo",
	//			},
	//		}
	//	};
	//}

	//public interface IFoo : IState {
	//	string Foo { get; set; }
	//	int ClickCount { get; set; }
	//}
	//public class MainPage2 : StateHotPage<IFoo> {
	//	protected override void CreateState (IFoo state)
	//	{
	//		state.Foo = "Hello";
	//		state.ClickCount = 1;
	//	}

	//	protected override View Build (IFoo state) => new Stack {
	//		new Label {
	//			Text = state.Foo,
	//		},
	//		new Button {
	//			Text = "Click Me",
	//			OnClick =()=>{
	//				state.Foo = $"Clicked: {state.ClickCount++}";
	//			},
	//		}
	//	};
	//}

	/// <summary>
	/// This one lets you pass in any arbitrary list of view or cells. Great for a settings screen
	/// </summary>
	//public class ListPage : HotPage {
	//	protected override View Build () => new ListView {
	//			new Stack {
	//				new Label(){Text = "First Item",
	//			},
	//			new Stack {
	//				new Label{Text = "Second Item" },
	//			}
	//		}
	//	};
	//}

	//public class ListPage1 : HotPage {
	//	protected override View Build () => new ListView {
	//		ItemsSource = Enumerable.Range (0, 10),
	//		ViewFor = (x) => new Stack {
	//			new Label{Text = x.ToString() },
	//			new Label{Text = "Hi" },
	//		},
	//	};
	//}

	//public class ListPage2 : HotPage {
	//	class MyDataModel {
	//		public string Foo { get; set; } = "Foo";
	//		public string Bar { get; set; } = "Bar";
	//		public int Index { get; set; }
	//	}
	//	protected override View Build () => new ListView<MyDataModel> {
	//		ItemsSource = Enumerable.Range (0, 10).Select (x => new MyDataModel { Index = x }),
	//		ViewFor = (x) => new Stack {
	//			new Label{Text = $"Index: {x.Index}" },
	//			new Label{Text = $"Foo: {x.Foo}" },
	//			new Label{Text = $"Bar: {x.Bar}" },
	//		},
	//	};
	//}
}