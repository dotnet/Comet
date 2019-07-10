using AView = Android.Views.View;

namespace HotUI.Android
{
    public interface AndroidViewHandler : IViewHandler
    {
        AView View { get; }
    }
}