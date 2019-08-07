using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using HotUI.Skia;

namespace HotUI 
{
	public class DrawableControl : View, IDrawableControl
	{
		public IControlDelegate ControlDelegate { get; set; }
		
		public DrawableControl(IControlDelegate controlDelegate)
		{
			ControlDelegate = controlDelegate;
			controlDelegate.VirtualDrawableControl = this;
		}
		
		internal State GetState() => State;

		public void SetStateValue<T> (ref T currentValue, T newValue, [CallerMemberName] string propertyName = "")
		{
			State.SetValue (ref currentValue, newValue, this , propertyName);
		}
		

		protected override void ViewPropertyChanged(string property, object value)
		{
			ControlDelegate?.ViewPropertyChanged(property, value);
			base.ViewPropertyChanged(property, value);
		}
	}
	
	public class BindingDefinition
	{
		public BindingDefinition(Binding binding, string propertyName, Action<object> updater)
		{
			Binding = binding;
			PropertyName = propertyName;
			Updater = updater;
		}

		public Binding Binding { get; }
		public string PropertyName { get; }
		public Action<object> Updater {  get; }
	}
}
