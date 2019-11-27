namespace Comet.Skia
{
	public static class ControlDelegateExtensions
	{
		public static Comet.View ToView(this IControlDelegate controlDelegate)
		{
			return new DrawableControl(controlDelegate);
		}
	}
}
