using System;

namespace HotUI 
{
	public class Button : BoundControl<string>, ITextView
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
		
		public Action OnClick { get; private set; }

        public string TextValue => BoundValue;
    }
}
