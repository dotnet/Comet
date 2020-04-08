using System;
using System.Maui.iOS.Controls;
using UIKit;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace System.Maui.iOS.Handlers
{
	public class SpacerHandler : AbstractHandler<Spacer, UIView>
	{
		protected override UIView CreateView() => NativeView as UIView ?? new UIView();
	}
}
