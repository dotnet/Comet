using System;
using System.Linq;
using HotUI.iOS.Handlers;

namespace HotUI.iOS
{
    public class TabViewHandler : AbstractHandler<TabView,HUITabView>
    {
        public override bool AutoSafeArea => false;
        public override void SetView(View view)
        {
            base.SetView(view);
            TypedNativeView?.Setup(VirtualView?.ToList());
            SubscribeEvent();


        }
        void SubscribeEvent()
        {
            if (VirtualView == null)
                return;
            VirtualView.ChildrenChanged += VirtualView_ChildrenChanged;
            VirtualView.ChildrenAdded += VirtualView_ChildrenChanged;
            VirtualView.ChildrenRemoved += VirtualView_ChildrenChanged;
        }

        private void VirtualView_ChildrenChanged(object sender, LayoutEventArgs e)
        {
            TypedNativeView?.Setup(VirtualView?.ToList());
        }

        void UnsubscribeEvents()
        {
            if (VirtualView == null)
                return;
            VirtualView.ChildrenChanged -= VirtualView_ChildrenChanged;
            VirtualView.ChildrenAdded -= VirtualView_ChildrenChanged;
            VirtualView.ChildrenRemoved -= VirtualView_ChildrenChanged;
        }
       
        public override void Remove(View view)
        {
            UnsubscribeEvents();
            base.Remove(view);
        }
        protected override HUITabView CreateView() => new HUITabView();
    }
}
