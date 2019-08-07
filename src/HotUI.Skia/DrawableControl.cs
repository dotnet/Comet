using System;
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

			if (controlDelegate is AbstractControlDelegate abstractControlDelegate)
				AddBindings(abstractControlDelegate.Bindings);
		}
		
		public void SetStateValue<T> (ref T currentValue, T newValue, [CallerMemberName] string propertyName = "")
		{
			State.SetValue (ref currentValue, newValue, this , propertyName);
		}
	}
}
