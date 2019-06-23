using System;
namespace HotUI.Samples {
	public class MainPage : HotPage {
		class MyBindingObject : BindingObject {
			public bool CanEdit {
				get => GetProperty<bool> ();
				set => SetProperty (value);
			}
			public string Text {
				get => GetProperty<string> ();
				set => SetProperty (value);
			}
		}

		[State]
		readonly MyBindingObject state;

		readonly State<int> clickCount = new State<int> (1);

		readonly State<bool> bar = new State<bool> ();

		public MainPage ()
		{
			Title = "Hello HotUI";
			state = new MyBindingObject {
				Text = "Bar",
				CanEdit = true,
			};
		}

		protected override View Build () =>
			//new ScrollView{
				new Stack {
			 (state.CanEdit ?
				(View)new Entry {
					Text = state.Text,
					Completed =(e)=> state.Text = e
				}
				: new Label { FormatedText = () =>  $"{state.Text}: multiText" }),// Fromated Text will warn you. This should be done by TextBinding
				new Label {Text = state.Text},
				new Button{Text = "Toggle Entry/Label", OnClick = ()=> state.CanEdit = !state.CanEdit},
				new Button{Text = "Update Text", OnClick = ()=>{
						state.Text = $"Click Count: {clickCount.Value++}";
					}
				}
			//}
			};
	}
}
