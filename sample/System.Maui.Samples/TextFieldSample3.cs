using System;
using System.Collections.Generic;

namespace System.Maui.Samples
{
	public class TextFieldSample3 : View
	{
		class MyBindingObject : BindingObject
		{
			public string Text
			{
				get => GetProperty<string>();
				set => SetProperty(value);
			}
		}

		[State] private readonly MyBindingObject _state = new MyBindingObject { Text = "Edit Me" };

		[Body]
		View Build() => new VStack()
		{
			new Entry(_state.Text, "Name"),
			new HStack()
			{
				new Label("Current Value:")
					.Color(Color.Grey),
				new Label(_state.Text),
				new Spacer()
			},
		}.FillHorizontal();
	}

}
