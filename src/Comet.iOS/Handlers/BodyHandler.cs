using System;
using Comet.iOS.Handlers;
using UIKit;

namespace Comet.iOS
{
	public class BodyHandler : AbstractHandler<View, CometView>
	{
		protected override CometView CreateView() => new CometView();
		public override void SetView(View view)
		{
			base.SetView(view);
			TypedNativeView.CurrentView = view;
		}
	}
}
