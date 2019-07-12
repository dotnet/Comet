using System;

namespace HotUI 
{
	public class Slider : View
	{
		public Slider (
			float value, 
			float from = 0, 
			float through = 100, 
			float by = 1) : base (true)
		{
			Value = value;
			From = from;
			Through = through;
			By = by;

			//OnEditingChanged = newValue => _state.Value = newValue;
		}
		
		public Slider (
			Func<float> value, 
			float from = 0, 
			float through = 100, 
			float by = 1)
		{
			ValueBinding = value;
			From = from;
			Through = through;
			By = by;
		}
		
		public Slider (
			State<float> value, 
			float from = 0, 
			float through = 100, 
			float by = 1) : base (true)
		{
			Value = (float)value;
			From = from;
			Through = through;
			By = by;

			OnEditingChanged = newValue => value.Value = newValue;
		}
		
		float _value;
		public float Value {
			get => _value;
			private set => this.SetValue (State, ref this._value, value, ViewPropertyChanged);
		}

		float _from;
		public float From {
			get => _from;
			set => this.SetValue (State, ref _from, value, ViewPropertyChanged);
		}
		
		float _through;
		public float Through {
			get => _through;
			set => this.SetValue (State, ref _through, value, ViewPropertyChanged);
		}

		float _by;
		public float By {
			get => _by;
			set => this.SetValue (State, ref _by, value, ViewPropertyChanged);
		}
		
		public Func<float> ValueBinding { get; private set; }
		protected override void WillUpdateView ()
		{
			base.WillUpdateView ();
			if (ValueBinding != null) {
				State.StartProperty ();
				var text = ValueBinding.Invoke ();
				var props = State.EndProperty ();
				var propCount = props.Length;
				if (propCount > 0) {
					State.BindingState.AddViewProperty (props, (s,o)=> Value = ValueBinding.Invoke());
				}
				Value = text;
			}
		}
		
		public Action<float> OnEditingChanged { get; private set; }
	}
}
