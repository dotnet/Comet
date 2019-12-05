using System;
using System.Collections.Generic;

namespace Comet
{
	public static class AnimationExtensions
	{

		public static T Animate<T>(this T view, Action<T> action, Action completed = null, double duration = .2, double delay = 0, bool repeats = false, bool autoReverses = false, string id = null, Lerp lerp = null)
			where T : View => view.Animate(Easing.Default, action, completed, duration, delay, repeats, autoReverses, id, lerp);

		public static T Animate<T>(this T view, Easing easing, Action<T> action, Action completed = null, double duration = .2, double delay = 0, bool repeats = false, bool autoReverses = false, string id = null, Lerp lerp = null)
		where T : View
		{
			var animation = CreateAnimation(view, easing, action, completed, duration, delay, repeats, autoReverses, id, lerp);
			view.AddAnimation(animation);
			return view;
		}

		public static AnimationSequence<T> BeginAnimationSequence<T>(this T view, Action completed = null, double delay = 0, bool repeats = false, string id = null)
			where T : View
				=> new AnimationSequence<T>(view)
				{
					StartDelay = delay,
					Repeats = repeats,
					Id = id,
				};

		public static Animation CreateAnimation<T>(T view, Easing easing, Action<T> action, Action completed = null, double duration = .2, double delay = 0, bool repeats = false, bool autoReverses = false, string id = null, Lerp lerp = null)
			where T : View
		{
			ContextualObject.MonitorChanges();
			action(view);
			var changedProperties = ContextualObject.StopMonitoringChanges();
			List<Animation> animations = null;
			if (changedProperties.Count == 0)
				return null;

			if (changedProperties.Count > 1)
				animations = new List<Animation>();

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
					ContextualObject = prop.view,
					PropertyName = prop.property,
					Id = id,
					Lerp = lerp,
					
				};
				if (autoReverses)
					animation = animation.CreateAutoReversing();
				if (animations == null)
					return animation;
				animations.Add(animation);
			}

			return new Animation(animations)
			{
				Id = id,
				Duration = duration,
				Easing = easing,
				Repeats = repeats,
			};
		}

		public static Color Lerp(this Color color, double progress, Color endColor)
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
	}
}
