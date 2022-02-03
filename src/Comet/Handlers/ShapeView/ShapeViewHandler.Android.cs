using System;
using Microsoft.Maui.Handlers;
using Microsoft.Maui;
using Microsoft.Maui.Graphics.Platform;

namespace Comet.Handlers
{
	public partial class ShapeViewHandler : ViewHandler<ShapeView, PlatformGraphicsView>
	{
		protected override PlatformGraphicsView CreateNativeView() => new PlatformGraphicsView(MauiContext.Context);


		public static void MapShapeProperty(IElementHandler viewHandler, ShapeView virtualView)
		{
			var nativeView = (PlatformGraphicsView)viewHandler.NativeView;
			nativeView.Drawable = virtualView;
		}
	}
}
