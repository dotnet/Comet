using System;
using Comet.iOS.Controls;
using UIKit;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Comet.iOS.Handlers
{
	public class SpacerHandler : AbstractHandler<Spacer, UIView>
	{
		protected override UIView CreateView() => NativeView as UIView ?? new UIView();
	}
}
