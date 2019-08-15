namespace Comet
{
    public class FrameConstraints
    {
        public FrameConstraints(float? width, float? height, Alignment alignment)
        {
            Width = width;
            Height = height;
            Alignment = alignment ?? Alignment.Center;
        }

        public float? Width { get; }
        public float? Height { get; }
        public Alignment Alignment { get; }
    }
}