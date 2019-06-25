    using System;

namespace HotUI.Samples
{
    public class MainPage : View
    {
        class MyBindingObject : BindingObject
        {
            public bool CanEdit
            {
                get => GetProperty<bool>();
                set => SetProperty(value);
            }

            public string Text
            {
                get => GetProperty<string>();
                set => SetProperty(value);
            }
        }

        [State]
        readonly MyBindingObject state;

        readonly State<int> clickCount = new State<int>(1);

        readonly State<bool> bar = new State<bool>();

        public MainPage()
        {
            state = new MyBindingObject
            {
                Text = "Bar",
                CanEdit = true,
            };
			Body = Build;
        }

        View Build () =>
            new ScrollView
            {
                new Stack
                {
                    (state.CanEdit
                        ? (View) new TextField(()=>state.Text)
                        {
                            Completed = (e) => state.Text = e
                        }
                        : new Text(() => $"{state.Text}: multiText")), // Fromated Text will warn you. This should be done by TextBinding
                    new Text(state.Text),
                    new Button("Toggle Entry/Label") {
						OnClick = () => state.CanEdit = !state.CanEdit},
                    new Button("Update Text")
                    {
                        OnClick = () => { state.Text = $"Click Count: {clickCount.Value++}"; }
                    }
                }
            };
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