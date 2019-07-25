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

            // Check if unsupported as this can cause infinite recursion if not checked
            if (View.IsIUnsupportednternalView())
            {
                builder.AddContent(2, $"Unsupported view: {View.GetType()}");
            }
            else if (View?.GetOrCreateViewHandler() is IBlazorViewHandler handler)
            {
                builder.OpenComponent(3, handler.Component);
                builder.SetKey(handler);
                builder.AddComponentReferenceCapture(4, handler.OnComponentLoad);
                builder.CloseComponent();
            }
            else
            {
                builder.AddContent(5, "Error: No view");
            }

            builder.CloseElement();
        }

        [Parameter]
        public View View { get; set; }
    }
}
