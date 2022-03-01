using System;
using Microsoft.Maui.Handlers;
//using Microsoft.Maui.Graphics.Platform;
using Microsoft.Maui;
using Microsoft.UI.Xaml.Controls;

namespace Comet.Handlers
{
	public partial class ShapeViewHandler : ViewHandler<ShapeView, Panel>
	{
		protected override Panel CreatePlatformView() => new LayoutPanel();


		public static void MapShapeProperty(IElementHandler viewHandler, ShapeView virtualView)
		{
			//var nativeView = (PlatformGraphicsView)viewHandler.PlatformView;
			//nativeView.Drawable = virtualView;
		}
	}
}
