using System;
using Microsoft.Maui.Handlers;
using Microsoft.Maui;
namespace Comet.GraphicsControls
{
	public partial class GraphicsControlHandler<TViewDrawable, TVirtualView> : ViewHandler<TVirtualView, object>
	{
		protected override object CreateNativeView() => throw new NotImplementedException();
		public void Invalidate() => throw new NotImplementedException();

	}
}
