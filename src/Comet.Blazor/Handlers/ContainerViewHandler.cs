using Comet.Blazor.Components;

namespace Comet.Blazor.Handlers
{
    internal class ContainerViewHandler : BlazorHandler<ContainerView, BContainerView>
    {
        public static readonly PropertyMapper<ContainerView> Mapper = new PropertyMapper<ContainerView>
        {
        };

        public ContainerViewHandler() : base(Mapper)
        {
        }

        public override void SetView(View view)
        {
            if (NativeView != null)
            {
                NativeView.Views = (ContainerView)view;
            }

            base.SetView(view);
        }

        protected override void NativeViewUpdated()
        {
            NativeView.Views = VirtualView;
        }
    }
}
