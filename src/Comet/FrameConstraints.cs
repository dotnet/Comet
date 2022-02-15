namespace Comet
{
	public class FrameConstraints
	{
		public FrameConstraints(float? width, float? height)
		{
			Width = width;
			Height = height;
		}

		public float? Width { get; }
		public float? Height { get; }
	}
}
