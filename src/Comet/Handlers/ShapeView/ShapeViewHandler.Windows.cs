using System;
using Microsoft.Maui.Handlers;
//using Microsoft.Maui.Graphics.Native;
using Microsoft.Maui;
using Microsoft.UI.Xaml.Controls;

namespace Comet.Handlers
{
	public partial class ShapeViewHandler : ViewHandler<ShapeView, Panel>
	{
		protected override Panel CreateNativeView() => new LayoutPanel();


		public static void MapShapeProperty(IElementHandler viewHandler, ShapeView virtualView)
		{
			//var nativeView = (NativeGraphicsView)viewHandler.NativeView;
			//nativeView.Drawable = virtualView;
		}
	}
}
