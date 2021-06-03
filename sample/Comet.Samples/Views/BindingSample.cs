using System;
namespace Comet.Samples
{
	public class BindingSample : View
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

		readonly State<int> clickCount = 1;

		readonly State<bool> bar = false;

		public BindingSample()
		{
			state = new MyBindingObject
			{
				Text = "Bar",
				CanEdit = true,
			};
			Body = Build;
		}

		View Build() =>
			new NavigationView{ new ScrollView
			{
				new VStack
				{
					(state.CanEdit
						? (View) new TextField(state.Text)
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
								var font = View.GetGlobalEnvironment<float?>(EnvironmentKeys.Fonts.Size) ?? 14;
								var size = font + 5;
								View.SetGlobalEnvironment (EnvironmentKeys.Fonts.Size, size);
							}),
					},
					new Toggle(state.CanEdit)
				}
			}
		};
	}
}
