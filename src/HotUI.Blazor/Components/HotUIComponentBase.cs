using Microsoft.AspNetCore.Components;

namespace HotUI.Blazor.Components
{
    public abstract class HotUIComponentBase : ComponentBase
    {
        private protected HotUIComponentBase()
        {
        }

        internal void NotifyUpdate() => base.StateHasChanged();
    }
}
