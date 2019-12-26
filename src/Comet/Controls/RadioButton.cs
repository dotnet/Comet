using System;

namespace Comet
{
	public class RadioButton : View
	{
		public RadioButton(
			Binding<string> label = null,
			Binding<bool> selected = null,
			Binding<string> groupName = null,
			Action onClick = null)
		{
			Label = label;
			Selected = selected;
			GroupName = groupName;
			OnClick = onClick;
		}

		public RadioButton(
			Func<string> label,
			Func<bool> selected = null,
			Func<string> groupName = null,
			Action onClick = null)
			: this(
				  (Binding<string>)label,
				  (Binding<bool>)selected,
				  (Binding<string>)groupName,
				  onClick)
		{

		}

		Binding<string> _label;
		public Binding<string> Label
		{
			get => _label;
			private set => this.SetBindingValue(ref _label, value);
		}

		Binding<bool> _selected;
		public Binding<bool> Selected
		{
			get => _selected;
			private set => this.SetBindingValue(ref _selected, value);
		}

		Binding<string> _groupName;
		public Binding<string> GroupName
		{
			get => _groupName;
			private set => this.SetBindingValue(ref _groupName, value);
		}

		public Action OnClick { get; private set; }
	}
}
 