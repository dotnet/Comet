using System;

namespace HotUI 
{
	public class Slider : BoundView<float>
	{
		public Slider (
			Binding<float> value = null, 	
			float from = 0, 
			float through = 100, 
			float by = 1,
			Action<float> onEditingChanged = null) : base(value, nameof(Value))
		{
			From = from;
			Through = through;
			By = by;
			OnEditingChanged = new MulticastAction<float>(value, onEditingChanged);
		}
		
		public float Value {
			get => BoundValue;
			private set => BoundValue = value;
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
		
		public Action<float> OnEditingChanged { get; private set; }
	}
}
