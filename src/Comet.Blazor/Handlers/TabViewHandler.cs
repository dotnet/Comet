using Comet.Blazor.Components;

namespace Comet.Blazor.Handlers
{
    internal class TabViewHandler : BlazorHandler<TabView, BTabView>
    {
        public static readonly PropertyMapper<TabView> Mapper = new PropertyMapper<TabView>
        {
        };

        public TabViewHandler()
            : base(Mapper)
        {
        }

        protected override void NativeViewUpdated()
        {
            NativeView.Views = VirtualView;
            base.NativeViewUpdated();
        }
    }
}
