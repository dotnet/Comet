using Microsoft.Maui.Handlers;
using Microsoft.Maui;
using System;

namespace Comet.Handlers
{
	public partial class ImageHandler : ViewHandler<Image, object>
	{
		protected override object CreateNativeView() => throw new NotImplementedException();

		public static void MapBitmapProperty(IViewHandler viewHandler, Image virtualView)
		{

		}
	}
}
