using HotUI.Graphics;

namespace HotUI
{
    public class Path : Shape
    {
        private readonly PathF _path;

        public Path(PathF path)
        {
            _path = path;
        }

        public Path(string path)
        {
            _path = PathBuilder.Build(path);
        }
        
        public override PathF PathForBounds(RectangleF rect)
        {
            // todo: this should scale based on some type of setting (AspectFit, ScaleToFit, etc...)
            return _path;
        }
    }
}