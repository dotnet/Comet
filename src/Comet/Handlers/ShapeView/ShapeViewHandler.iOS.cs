using System;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Graphics.CoreGraphics;
using Microsoft.Maui;

namespace Comet.Handlers
{
	public partial class ShapeViewHandler : ViewHandler<ShapeView, NativeGraphicsView>
	{
		protected override NativeGraphicsView CreateNativeView() => new NativeGraphicsView();


		public static void MapShapeProperty(IViewHandler viewHandler, ShapeView virtualView)
		{
			var nativeView = (NativeGraphicsView)viewHandler.NativeView;
			nativeView.Drawable = virtualView;
		}
	}
}
