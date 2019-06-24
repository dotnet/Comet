using Android.App;
using AView = Android.Views.View;

namespace HotUI.Android
{
    public interface IView : IViewHandler
    {
        AView View { get; }
    }

    public interface IViewContainer : IViewBuilderHandler
    {
        AView View { get; }
    }
}