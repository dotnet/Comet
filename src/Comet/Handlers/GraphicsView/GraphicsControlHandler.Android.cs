using System;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Graphics.Native;
using Microsoft.Maui;
namespace Comet.GraphicsControls
{
	public partial class GraphicsControlHandler<TViewDrawable, TVirtualView> : ViewHandler<TVirtualView, NativeGraphicsView>
	{
		protected override NativeGraphicsView CreateNativeView() => new NativeGraphicsView(MauiContext.Context) { Drawable = this};
		public void Invalidate() => NativeView.Invalidate();
	}
}
