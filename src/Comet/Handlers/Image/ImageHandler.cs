using System;
using Microsoft.Maui.Handlers;
using Microsoft.Maui;
namespace Comet.Handlers
{
	public partial class ImageHandler
	{
		public static readonly PropertyMapper<Image> Mapper = new PropertyMapper<Image>(ViewHandler.ViewMapper)
		{
			[nameof(Image.Bitmap)] = MapBitmapProperty
		};



		public ImageHandler() : base(Mapper)
		{
		}

		public ImageHandler(PropertyMapper<Image> mapper) : base(mapper)
		{
		}
	}
}
