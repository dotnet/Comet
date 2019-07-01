using System;
namespace HotUI.Samples {
	public class BindingSample : View {
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

		public BindingSample ()
		{
			state = new MyBindingObject {
				Text = "Bar",
				CanEdit = true,
			};
			Body = Build;
		}

		View Build () =>
			new NavigationView{ new ScrollView
			{
				new VStack
				{
					(state.CanEdit
						? (View) new TextField(() => state.Text)
						{
							Completed = (e) => state.Text = e
						}
						: new Text(() => $"{state.Text}: multiText")), // Fromated Text will warn you. This should be done by TextBinding
                    new Text(state.Text),
					new HStack
					{
						new Button("Toggle Entry/Label")
						{
							OnClick = () => state.CanEdit = !state.CanEdit
						},
						new Button("Update Text")
						{
							OnClick = () => { state.Text = $"Click Count: {clickCount.Value++}"; }
						},
						new Button("Update FontSize")
						{
							OnClick = () => {

								var getFontSize =View.GetGlobalEnvironment<int>(EnvironmentKeys.Fonts.FontSize);
								getFontSize+=5;
								View.SetGlobalEnvironment (EnvironmentKeys.Fonts.FontSize, getFontSize);
							}
						},
					},
					new Toggle(() =>state.CanEdit)
					{
						IsOnChanged = (e) => state.CanEdit = e
					}
				}
			}
		};
	}
}
