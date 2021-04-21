using System;
using Microsoft.Maui.Handlers;
using Microsoft.Maui;
namespace Comet.Handlers
{
	public partial class GraphicsControlHandler<TViewHandler, TVirtualView> : ViewHandler<TVirtualView, object>
	{
		protected override object CreateNativeView() => throw new NotImplementedException();
		public void Invalidate() => throw new NotImplementedException();

	}
}
