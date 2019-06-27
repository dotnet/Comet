using Windows.UI.Xaml;

namespace HotUI.UWP
{
    public interface IUIElement : IViewHandler
    {
        UIElement View { get; }
    }
}