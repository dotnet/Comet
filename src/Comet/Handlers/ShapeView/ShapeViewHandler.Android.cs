using System;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Graphics.Android;
using Microsoft.Maui;

namespace Comet.Handlers
{
	public partial class ShapeViewHandler : ViewHandler<ShapeView, NativeGraphicsView>
	{
		protected override NativeGraphicsView CreateNativeView() => new NativeGraphicsView(MauiContext.Context);


		public static void MapShapeProperty(IViewHandler viewHandler, ShapeView virtualView)
		{
			var nativeView = (NativeGraphicsView)viewHandler.NativeView;
			nativeView.Drawable = virtualView;
		}
	}
}
