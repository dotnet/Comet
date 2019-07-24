using System;

namespace HotUI.Blazor.Handlers
{
    internal interface IBlazorViewHandler : IViewHandler
    {
        Type Component { get; }

        void OnComponentLoad(object nativeView);
    }
}
