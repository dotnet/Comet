using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace HotUI.Blazor
{
    public class HotUIView : ComponentBase
    {
        public override Task SetParametersAsync(ParameterCollection parameters)
        {
            View = parameters.GetValueOrDefault<View>(nameof(View));

            return base.SetParametersAsync(parameters);
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            builder.AddView(0, View);
        }

        [Parameter]
        public View View { get; private set; }
    }
}
