using HotUI.Blazor.Handlers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;

namespace HotUI.Blazor.Components
{
    public class BView : HotUIComponentBase
    {
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (View?.GetOrCreateViewHandler() is IBlazorViewHandler handler)
            {
                builder.OpenComponent(0, handler.Component);
                builder.AddComponentReferenceCapture(1, handler.SetNativeView);
                builder.CloseComponent();
            }
            else
            {
                builder.AddContent(2, "Error: No view");
            }
        }

        [Parameter]
        public View View { get; set; }
    }
}
