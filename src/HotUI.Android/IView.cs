using AView = Android.Views.View;

namespace HotUI.Android
{
    public interface IView : IViewHandler
    {
        AView View { get; }
    }
}