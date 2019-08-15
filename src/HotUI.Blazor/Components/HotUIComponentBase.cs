using Microsoft.AspNetCore.Components;

namespace Comet.Blazor.Components
{
    public abstract class CometComponentBase : ComponentBase
    {
        private protected CometComponentBase()
        {
        }

        internal void NotifyUpdate() => Invoke(StateHasChanged);
    }
}
