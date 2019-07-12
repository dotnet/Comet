using System;

namespace HotUI 
{
	public class Slider : View
	{
		public Slider (
			Binding<float> value = null, 
			float from = 0, 
			float through = 100, 
			float by = 1,
			Action<float> onEditingChanged = null) : base(value?.Implicit ?? false)
		{
			ValueBinding = value?.Get;
			From = from;
			Through = through;
			By = by;
			OnEditingChanged = new MulticastAction<float>(value?.Set, onEditingChanged);
		}
		
		float _value;
		public float Value {
			get => _value;
			private set => this.SetValue (State, ref this._value, value, ViewPropertyChanged);
		}

		float _from;
		public float From {
			get => _from;
			private set => this.SetValue (State, ref _from, value, ViewPropertyChanged);
		}
		
		float _through;
		public float Through {
			get => _through;
			private set => this.SetValue (State, ref _through, value, ViewPropertyChanged);
		}

		float _by;
		public float By {
			get => _by;
			private set => this.SetValue (State, ref _by, value, ViewPropertyChanged);
		}
		
		public Func<float> ValueBinding { get; private set; }
		protected override void WillUpdateView ()
		{
			base.WillUpdateView ();
			if (ValueBinding != null) 
			{
				State.StartProperty ();
				var text = ValueBinding.Invoke ();
				var props = State.EndProperty ();
				var propCount = props.Length;
				if (propCount > 0) 
					State.BindingState.AddViewProperty (props, (s,o)=> Value = ValueBinding.Invoke());

				Value = text;
			}
		}
		
		public Action<float> OnEditingChanged { get; }
	}
}
