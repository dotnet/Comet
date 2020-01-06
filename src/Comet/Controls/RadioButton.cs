using System;

namespace Comet
{
	public class RadioButton : View
	{
		public RadioButton(
			Binding<string> label = null,
			Binding<bool> selected = null,
			Action onClick = null)
		{
			Label = label;
			Selected = selected;
			OnClick = onClick;
		}

		public RadioButton(
			Func<string> label,
			Func<bool> selected = null,
			Action onClick = null)
			: this(
				  (Binding<string>)label,
				  (Binding<bool>)selected,
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

		public Action OnClick { get; private set; }

		protected override View GetRenderView()
		{
			View view =  base.GetRenderView();

			if (view.Parent is RadioGroup)
			{
				return view;
			}

			// TODO: Create Comet-specific UI exceptions
			throw new Exception("A RadioButton must be in a RadioGroup");
		}
	}
}
 