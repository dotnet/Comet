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

		protected override void ViewPropertyChanged(string property, object value)
		{
			ControlDelegate?.ViewPropertyChanged(property, value);
			base.ViewPropertyChanged(property, value);
		}
	}
}
