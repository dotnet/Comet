using System;

namespace HotUI 
{
	public class Button : BoundView<string> 
	{
		public Button (
			Binding<string> value = null, 	
			Action action = null) : base(value, nameof(Text))
		{
			OnClick = action;
		}
		
		public Button(
			Func<string> value,
			Action action = null) : this((Binding<string>)value, action)
		{
		}
		
		public string Text {
			get => BoundValue;
			private set => BoundValue = value;
		}
		
		public Action OnClick { get; private set; }
	}
}
