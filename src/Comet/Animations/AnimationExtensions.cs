using System;
namespace Comet
{
	public static class AnimationExtensions
	{

		public static T Animate<T>(this T view, Action<T> action, Action completed = null, double duration = .2, bool repeats = false)
			where T : View => view.Animate(Easing.CubicInOut, action, completed, duration, repeats);

		public static T Animate<T>(this T view, Easing easing, Action<T> action, Action completed = null, double duration = .2, bool repeats = false)
		where T : View
		{

			var oldFrame = view.Frame;

			ContextualObject.MonitorChanges();
			action(view);
			var changedProperties = ContextualObject.StopMonitoringChanges();

			foreach (var change in changedProperties)
			{
				var propertyName = change.Key;
				var values = change.Value;
				
				//new Animation(propertyName, values.oldValue, values.newValue);
			}
			return view;
		}

		public static Color Lerp(this Color color, double progress, Color endColor)
        {
			float Lerp(float start, float end, double progress) => (float)((start - end) * progress) + start; 

			var r = Lerp(color.R, endColor.R, progress);
			var b = Lerp(color.B, endColor.B, progress);
			var g = Lerp(color.G, endColor.G, progress);
			var a = Lerp(color.A, endColor.A, progress);
			return new Color(r, b, g, a);
		}
	}
}
