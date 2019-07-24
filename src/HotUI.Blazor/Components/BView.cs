using Microsoft.AspNetCore.Components.RenderTree;

namespace HotUI.Blazor.Components
{
    public class BView : HotUIComponentBase
    {
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            builder.AddView(0, View);
        }

        public View View { get; set; }
    }
}
