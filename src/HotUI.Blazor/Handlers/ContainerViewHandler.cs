using HotUI.Blazor.Components;

namespace HotUI.Blazor.Handlers
{
    internal class ContainerViewHandler : BlazorHandler<ContainerView, BContainerView>
    {
        public static readonly PropertyMapper<ContainerView> Mapper = new PropertyMapper<ContainerView>
        {
        };

        public ContainerViewHandler() : base(Mapper)
        {
        }

        protected override void NativeViewUpdated()
        {
            NativeView.Views = VirtualView;
        }
    }
}
