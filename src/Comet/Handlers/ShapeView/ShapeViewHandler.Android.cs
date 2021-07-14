using System;
using Microsoft.Maui.Handlers;
using Microsoft.Maui;
using Microsoft.Maui.Graphics.Native;

namespace Comet.Handlers
{
	public partial class ShapeViewHandler : ViewHandler<ShapeView, NativeGraphicsView>
	{
		protected override NativeGraphicsView CreateNativeView() => new NativeGraphicsView(MauiContext.Context);


		public static void MapShapeProperty(IElementHandler viewHandler, ShapeView virtualView)
		{
			var nativeView = (NativeGraphicsView)viewHandler.NativeView;
			nativeView.Drawable = virtualView;
		}
	}
}
