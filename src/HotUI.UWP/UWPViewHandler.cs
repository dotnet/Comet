using Windows.UI.Xaml;

namespace HotUI.UWP
{
    public interface UWPViewHandler : IViewHandler
    {
        UIElement View { get; }
    }
}