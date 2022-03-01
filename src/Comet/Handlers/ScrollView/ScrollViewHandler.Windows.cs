using System;
using Microsoft.Maui.Handlers;
using Microsoft.Maui;
using UWPScrollView = Microsoft.UI.Xaml.Controls.ScrollViewer;

namespace Comet.Handlers
{
	public partial class ScrollViewHandler : ViewHandler<ScrollView, UWPScrollView>
	{
		protected override UWPScrollView CreatePlatformView() =>  new UWPScrollView();

	}
}
