using System;
using System.Drawing;

namespace System.Maui
{
	public static class AnimationLerpingExtensions
	{
		public static Color Lerp(this Color color, Color endColor, double progress)
		{
			color ??= Color.Black;
			endColor ??= Color.Black;
			float Lerp(float start, float end, double progress) => (float)(((end - start) * progress) + start);

			var r = Lerp(color.R, endColor.R, progress);
			var b = Lerp(color.B, endColor.B, progress);
			var g = Lerp(color.G, endColor.G, progress);
			var a = Lerp(color.A, endColor.A, progress);
			return new Color(r, g, b, a);
		}

		public static SizeF Lerp(this SizeF start, SizeF end, double progress) =>
			new SizeF(start.Width.Lerp(end.Width, progress), start.Height.Lerp(end.Height, progress));

		public static PointF Lerp(this PointF start, PointF end, double progress) =>
			new PointF(start.X.Lerp(end.X, progress), start.Y.Lerp(end.Y, progress));

		public static RectangleF Lerp(this RectangleF start, RectangleF end, double progress)
			=> new RectangleF(start.Location.Lerp(end.Location, progress), start.Size.Lerp(end.Size, progress));


		public static float Lerp(this float start, float end, double progress) =>
			(float)((end - start) * progress) + start;

		//IF there is a null, we toggle at the half way. If both values are set, we can lerp
		public static float? Lerp(this float? start, float? end, double progress)
			=> start.HasValue && end.HasValue ? start.Value.Lerp(end.Value, progress) : start.GenericLerp(end, progress);

		public static T GenericLerp<T>(this T start, T end, double progress, double toggleThreshold = .5)
			=> progress < toggleThreshold ? start : end;

		public static FrameConstraints Lerp(this FrameConstraints start, FrameConstraints end, double progress)
			=> new FrameConstraints(width: start.Width.Lerp(end.Width, progress),
				height: start.Height.Lerp(end.Height, progress),
				alignment: start.Alignment.GenericLerp(end.Alignment, progress));

		public static Thickness Lerp(this Thickness start, Thickness end, double progress)
			=> new Thickness(
				start.Left.Lerp(end.Left, progress),
				start.Top.Lerp(end.Top, progress),
				start.Right.Lerp(end.Right, progress),
				start.Bottom.Lerp(end.Bottom, progress)
				);
	}
}
