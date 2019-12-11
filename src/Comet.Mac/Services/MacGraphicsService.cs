using CoreGraphics;
using Comet.Graphics;
using Comet.Services;
using System.Drawing;

namespace Comet.Mac.Services
{
	public class MacGraphicsService : IGraphicsService
	{
		public RectangleF GetPathBounds(PathF path)
		{
			var nativePath = path.NativePath as CGPath;
			if (nativePath == null)
			{
				nativePath = path.ToCGPath();
				path.NativePath = nativePath;
			}

			var bounds = nativePath.PathBoundingBox;
			return bounds.ToRectangleF();
		}
	}
}
