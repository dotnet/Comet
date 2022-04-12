using System;
using Microsoft.Maui;
using Microsoft.Maui.Handlers;

namespace Comet.Handlers
{
	public partial class TabViewHandler : ViewHandler<TabView, object>
	{
		protected override object CreatePlatformView() => throw new NotImplementedException();
	}
}
