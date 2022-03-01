using System;
using Microsoft.Maui.Handlers;
using Microsoft.Maui;
using Microsoft.Maui.Graphics.Platform;

namespace Comet.Handlers
{
	public partial class ShapeViewHandler : ViewHandler<ShapeView, PlatformGraphicsView>
	{
		protected override PlatformGraphicsView CreatePlatformView() => new PlatformGraphicsView(MauiContext.Context);


		public static void MapShapeProperty(IElementHandler viewHandler, ShapeView virtualView)
		{
			var nativeView = (PlatformGraphicsView)viewHandler.PlatformView;
			nativeView.Drawable = virtualView;
		}
	}
}
