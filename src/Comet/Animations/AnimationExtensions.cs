using System;
namespace Comet
{
	public static class AnimationExtensions
	{

		public static T Animate<T>(this T view, Action<T> action, Action completed = null, double duration = .2, double delay = 0, bool repeats = false, bool autoReverses = false)
			where T : View => view.Animate(Easing.CubicInOut, action, completed, duration,delay, repeats, autoReverses);

		public static T Animate<T>(this T view, Easing easing, Action<T> action, Action completed = null, double duration = .2, double delay = 0, bool repeats = false, bool autoReverses = false)
		where T : View
		{

			var oldFrame = view.Frame;

			ContextualObject.MonitorChanges();
			action(view);
			var changedProperties = ContextualObject.StopMonitoringChanges();

			foreach (var change in changedProperties)
			{
				var prop = change.Key;
				var values = change.Value;
				if (values.newValue == values.oldValue)
					continue;
				var animation = new Animation
				{
					Duration = duration,
					Easing = easing,
					Repeats = repeats,
					StartDelay = delay,
					StartValue = values.oldValue,
					EndValue = values.newValue,
					ValueChanged = (value) => {
						prop.view.SetEnvironment(prop.property, value, prop.cascades);
					}
				};
				if (autoReverses)
					animation = animation.CreateAutoReversing();
				AnimationManger.Add(animation);
				//new Animation(propertyName, values.oldValue, values.newValue);
			}
			return view;
		}

		public static Color Lerp(this Color color, double progress, Color endColor)
        {
			float Lerp(float start, float end, double progress) => (float)(((end - start) * progress) + start); 

			var r = Lerp(color.R, endColor.R, progress);
			var b = Lerp(color.B, endColor.B, progress);
			var g = Lerp(color.G, endColor.G, progress);
			var a = Lerp(color.A, endColor.A, progress);
			return new Color(r, g, b,  a);
		}
	}
}
