namespace HotUI.Skia
{
    public static class ControlDelegateExtensions
    {
        public static HotUI.View ToView(this IControlDelegate controlDelegate)
        {
            return new DrawableControl(controlDelegate);
        }
    }
}