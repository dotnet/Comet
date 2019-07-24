using Microsoft.AspNetCore.Components.RenderTree;
using System.Collections.Generic;
using System.Linq;

namespace HotUI.Blazor.Components
{
    public class BContainerView : HotUIComponentBase
    {
        private IEnumerable<View> views;

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            var seq = 0;

            builder.OpenElement(seq++, "div");

            foreach (var view in Views)
            {
                builder.AddView(ref seq, view);
            }

            builder.CloseElement();
        }

        public IEnumerable<View> Views
        {
            get => views ?? Enumerable.Empty<View>();
            set => views = value;
        }
    }
}
