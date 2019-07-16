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
						? (View) new TextField(()=>state.Text, onCommit: (value) => state.Text = value )
						: new Text(() => $"{state.Text}: multiText")), // Formatted Text will warn you. This should be done by TextBinding
                    new Text(state.Text),
					new HStack
					{
						new Button("Toggle Entry/Label",
							() => state.CanEdit = !state.CanEdit),
						new Button("Update Text",
							() => state.Text = $"Click Count: {clickCount.Value++}"),
						new Button("Update FontSize",
							() => {

								var font = View.GetGlobalEnvironment<Font>(EnvironmentKeys.Fonts.Font) ?? Font.System(14);
								var size = font.Attributes.Size + 5;
								var newFont = Font.System(size);
								View.SetGlobalEnvironment (EnvironmentKeys.Fonts.Font, newFont);
							}),
					},
					new Toggle(state.CanEdit, e => state.CanEdit = e)
				}
			}
		};
	}
}
