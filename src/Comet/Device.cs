using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Comet.Services;

namespace Comet
{
	public static class Device
	{
		public static IFontService FontService = new FallbackFontService();
		public static IGraphicsService GraphicsService;
		public static IBitmapService BitmapService;
	}
}
