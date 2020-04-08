using System;
using System.Linq;
using System.Maui.iOS.Handlers;

namespace System.Maui.iOS
{
	public class TabViewHandler : AbstractHandler<TabView, CUITabView>
	{
		public override bool IgnoreSafeArea => VirtualView?.GetIgnoreSafeArea(true) ?? true;
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
		protected override CUITabView CreateView()
			=> NativeView as CUITabView ?? new CUITabView();
	}
}
