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
                builder.OpenElement(2, "div");
                builder.AddAttribute(3, "class", "alert alert-warning");
                builder.AddAttribute(4, "role", "alert");
                builder.AddMarkupContent(5, $"Unsupported view: <b>{View.GetType()}</b>");
                builder.CloseElement();
            }
            else if (View?.GetOrCreateViewHandler() is IBlazorViewHandler handler)
            {
                builder.OpenComponent(6, handler.ComponentType);
                builder.SetKey(handler);
                builder.AddComponentReferenceCapture(4, handler.OnComponentLoad);
                builder.CloseComponent();
            }
            else
            {
                builder.OpenElement(7, "div");
                builder.AddAttribute(8, "class", "alert alert-danger");
                builder.AddAttribute(9, "role", "alert");
                builder.AddMarkupContent(10, $"Invalid view handler: <b>{View.GetType()}</b>");
                builder.CloseElement();
            }

            builder.CloseElement();
        }

        [Parameter]
        public View View { get; set; }
    }
}
