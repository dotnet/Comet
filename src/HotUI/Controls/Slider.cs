using System;

namespace HotUI 
{
	public class Slider : BoundControl<float>
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
		
		public Slider (
			Func<float> value, 	
			float from = 0, 
			float through = 100, 
			float by = 1,
			Action<float> onEditingChanged = null) : this((Binding<float>)value, from, through, by, onEditingChanged)
		{

		}
		
		public float Value {
			get => BoundValue;
			private set => BoundValue = value;
		}

		float _from;
		public float From {
			get => _from;
			private set => SetValue ( ref _from, value);
		}
		
		float _through;
		public float Through {
			get => _through;
			private set => SetValue ( ref _through, value);
		}

		float _by;
		public float By {
			get => _by;
			private set => SetValue ( ref _by, value);
		}
		
		public Action<float> OnEditingChanged { get; private set; }
	}
}
