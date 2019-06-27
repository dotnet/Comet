using System.Windows;

namespace HotUI.WPF
{
    public interface IUIElement : IViewHandler
    {
        UIElement View { get; }
    }
}