using AppKit;
using System.Maui.Mac.Controls;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace System.Maui.Mac.Handlers
{
	public class SpacerHandler : AbstractHandler<Spacer, NSColorView>
	{
		protected override NSColorView CreateView()
		{
			return new NSColorView();
		}
	}
}
