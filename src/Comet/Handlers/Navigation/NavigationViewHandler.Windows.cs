using System;
using Microsoft.Maui.Handlers;
using Microsoft.Maui;
using Microsoft.UI.Xaml.Controls;

namespace Comet.Handlers
{
	public partial class NavigationViewHandler : ViewHandler<ScrollView, Panel>
	{
		protected override Panel CreatePlatformView() => new LayoutPanel();

	}
}
