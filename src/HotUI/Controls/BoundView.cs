using System;
using System.Runtime.CompilerServices;

namespace HotUI 
{
	public abstract class BoundView<T> : View
	{
		private readonly Binding<T> _binding;
		private readonly string _propertyName;
		
		protected BoundView (Binding<T> binding, string propertyName)
		{
			_binding = binding;
			_propertyName = propertyName;
		}
		
		T _boundValue;
		public T BoundValue 
		{
			get => _boundValue;
			protected set => this.SetValue (State, ref this._boundValue, value, ViewPropertyChanged, _propertyName);
		}
		
		protected void SetValue<T> (ref T currentValue, T newValue, [CallerMemberName] string propertyName = "")
		{
			State.SetValue<T> (ref currentValue, newValue, ViewPropertyChanged, propertyName);
		}
		
		protected override void WillUpdateView ()
		{
			base.WillUpdateView ();
			
			if (_binding.Get != null) 
			{
				State.StartProperty ();
				var value = _binding.Get.Invoke ();
				var props = State.EndProperty ();
				var propCount = props.Length;
				if (propCount > 0) 
					State.BindingState.AddViewProperty (props, (s, o) => BoundValue = _binding.Get.Invoke ());

				BoundValue = value;
			}
		}
	}
}
