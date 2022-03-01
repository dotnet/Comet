using System;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Graphics.Platform;
using Microsoft.Maui;

namespace Comet.Handlers
{
	public partial class ShapeViewHandler : ViewHandler<ShapeView, PlatformGraphicsView>
	{
		protected override PlatformGraphicsView CreatePlatformView() => new PlatformGraphicsView();


		public static void MapShapeProperty(IElementHandler viewHandler, ShapeView virtualView)
		{
			var nativeView = (PlatformGraphicsView)viewHandler.PlatformView;
			nativeView.Drawable = virtualView;
		}
	}
}
