namespace System.Maui.Skia
{
	public static class ControlDelegateExtensions
	{
		public static System.Maui.View ToView(this IControlDelegate controlDelegate)
		{
			return new DrawableControl(controlDelegate);
		}
	}
}
