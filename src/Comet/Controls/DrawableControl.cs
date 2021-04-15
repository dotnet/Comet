using Comet.Graphics;
using Microsoft.Maui.Graphics;

namespace Comet
{
	public class DrawableControl : View, IDrawableControl
	{
		public IControlDelegate ControlDelegate { get; set; }

		public DrawableControl(IControlDelegate controlDelegate)
		{
			ControlDelegate = controlDelegate;
			controlDelegate.VirtualDrawableControl = this;
		}

		//internal BindingState GetState() => State;

		public override void ViewPropertyChanged(string property, object value)
		{
			ControlDelegate?.ViewPropertyChanged(property, value);
			base.ViewPropertyChanged(property, value);
		}
	}
}
