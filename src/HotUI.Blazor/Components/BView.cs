using HotUI.Blazor.Handlers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;

namespace HotUI.Blazor.Components
{
    public class BView : HotUIComponentBase
    {
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "class", "hotui-view");

            if (View?.GetOrCreateViewHandler() is IBlazorViewHandler handler)
            {
                builder.OpenComponent(2, handler.Component);
                builder.SetKey(handler);
                builder.AddComponentReferenceCapture(3, handler.OnComponentLoad);
                builder.CloseComponent();
            }
            else
            {
                builder.AddContent(4, "Error: No view");
            }

            builder.CloseElement();
        }

        [Parameter]
        public View View { get; set; }
    }
}
