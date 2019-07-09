using System.Windows;

namespace HotUI.WPF
{
    public interface WPFViewHandler : IViewHandler
    {
        UIElement View { get; }
    }
}