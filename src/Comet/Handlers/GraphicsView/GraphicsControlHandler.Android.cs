using System;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Graphics.Native;
using Microsoft.Maui;
namespace Comet.Handlers
{
	public partial class GraphicsControlHandler<TViewHandler, TVirtualView> : ViewHandler<TVirtualView, NativeGraphicsView>
	{
		protected override NativeGraphicsView CreateNativeView() => new NativeGraphicsView(MauiContext.Context);
		public void Invalidate() => NativeView.Invalidate();
	}
}
