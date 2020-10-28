using System;
using Android.Content;
using Android.Views;
using Android.Widget;
using Xamarin.Platform;
using AView = Android.Views.View;
namespace Comet
{
	public class CometViewContainer : FrameLayout
	{
		public CometViewContainer(Context context) : base(context)
		{
			this.context = context;
		}

		AView childView;
		public void SetView(IView view)
		{
			if (childView != null)
				this.RemoveView(childView);
			childView = view.ToNative(context);
			childView.LayoutParameters = new ViewGroup.LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent);
			AddView(childView);
		}
		private readonly Context context;

	}
}
