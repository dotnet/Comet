using Microsoft.AspNetCore.Components.RenderTree;

namespace HotUI.Blazor.Components
{
    public class BView : HotUIComponentBase
    {
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            if (View is null)
            {
                builder.AddContent(0, $"Empty view");
            }
            else if (View.ViewHandler is null)
            {
                builder.AddView(0, View);
            }
            else
            {
                builder.AddContent(0, $"Unknown component: {View.GetType()}");
            }
        }

        public View View { get; set; }
    }
}
