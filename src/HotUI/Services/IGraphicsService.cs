using HotUI.Graphics;

namespace HotUI.Services
{
    public interface IGraphicsService
    {
        RectangleF GetPathBounds(PathF path);
    }
}