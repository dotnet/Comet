using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using HotUI.Reflection;

namespace HotUI 
{
	public abstract class BoundControl<T> : Control
	{
		private readonly Binding<T> _binding;
		private readonly string _propertyName;
		
		protected BoundControl (Binding<T> binding, string propertyName) : base(binding?.IsValue ?? false)
        {
            _propertyName = propertyName;
            _binding = binding;
            if (_binding != null)
            {
                BoundValue = _binding.CurrentValue;
                _binding.View = this;
                if(_binding.BoundProperties?.Length > 0)
                    _binding.BindToProperty(State, this, propertyName);
            }

		}
		
		T _boundValue;
		protected T BoundValue 
		{
			get => _boundValue;
			set
            {
                if (EqualityComparer<T>.Default.Equals(_boundValue, value))
                    return;
                _boundValue = value;

                try
                {
                    this.SetPropertyValue(_propertyName, value);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error setting property:{_propertyName} : {value}");
                    Debug.WriteLine(ex);
                }
            }
		}
		
		protected void SetValue<T> (ref T currentValue, T newValue, [CallerMemberName] string propertyName = "")
		{
			State.SetValue (ref currentValue, newValue, this , propertyName);
		}
		
        protected override void ViewPropertyChanged(string property, object value)
        {
            if(property == nameof(BoundValue) || property == _propertyName)
            {
                BoundValue = _binding.Get.Invoke();
            }
            base.ViewPropertyChanged(property, value);
        }
    }
}
