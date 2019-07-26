using HotUI.Blazor.Components;

namespace HotUI.Blazor.Handlers
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
