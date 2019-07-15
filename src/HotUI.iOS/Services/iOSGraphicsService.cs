using CoreGraphics;
using HotUI.Graphics;
using HotUI.Services;

namespace HotUI.iOS.Services
{
    public class iOSGraphicsService : IGraphicsService
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