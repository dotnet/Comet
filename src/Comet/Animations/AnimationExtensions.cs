using System;
using System.Collections.Generic;
using Microsoft.Maui;
using Microsoft.Maui.Animations;

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
				//Handle the bingings!
				if (values.newValue is Binding nb)
					values.newValue = nb.Value;
				if (values.oldValue is Binding ob)
					values.oldValue = ob.Value;
				if (values.newValue == values.oldValue)
					continue;
				Animation animation = new ContextualAnimation
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

			return new ContextualAnimation(animations)
			{
				Id = id,
				Duration = duration,
				Easing = easing,
				Repeats = repeats,
			};
		}
	}
}
