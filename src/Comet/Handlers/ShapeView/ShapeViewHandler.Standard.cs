using System;
using Microsoft.Maui.Handlers;
using Microsoft.Maui;
namespace Comet.Handlers
{
	public partial class ShapeViewHandler : ViewHandler<ShapeView, object>
	{
		protected override object CreateNativeView() => throw new NotImplementedException();

		public static void MapShapeProperty(IElementHandler viewHandler, ShapeView virtualView) { }
	}
}
