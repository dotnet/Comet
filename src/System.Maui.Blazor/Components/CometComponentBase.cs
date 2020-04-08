using Microsoft.AspNetCore.Components;

namespace System.Maui.Blazor.Components
{
    public abstract class System.MauiComponentBase : ComponentBase
    {
        private protected System.MauiComponentBase()
        {
        }

        internal void NotifyUpdate() => InvokeAsync(StateHasChanged);
    }
}
