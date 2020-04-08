using System.Maui.Blazor.Handlers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace System.Maui.Blazor.Components
{
    public class BView : System.MauiComponentBase
    {
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            var name = View?.GetType().Name ?? "null";
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "class", $"System.Maui-view System.Maui-view-{name}");

            if (View is null)
            {
                builder.OpenElement(2, "div");
                builder.AddAttribute(3, "class", "alert alert-danger");
                builder.AddAttribute(4, "role", "alert");
                builder.AddMarkupContent(5, "View cannot be null.");
                builder.CloseElement();
            }
            else if (View.GetOrCreateViewHandler() is IBlazorViewHandler handler)
            {
                builder.OpenComponent(6, handler.ComponentType);
                builder.SetKey(handler);
                builder.AddComponentReferenceCapture(7, handler.OnComponentLoad);
                builder.CloseComponent();
            }
            else
            {
                builder.OpenElement(8, "div");
                builder.AddAttribute(9, "class", "alert alert-danger");
                builder.AddAttribute(10, "role", "alert");
                builder.AddMarkupContent(11, $"Invalid view handler: <b>{View.GetType()}</b>");
                builder.CloseElement();
            }

            builder.CloseElement();
        }

        [Parameter]
        public View View { get; set; }
    }
}
