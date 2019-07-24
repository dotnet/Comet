using Microsoft.AspNetCore.Components;

namespace HotUI.Blazor.Components
{
    public class HotUIComponentBase : ComponentBase
    {
        internal void NotifyUpdate() => base.StateHasChanged();
    }
}
