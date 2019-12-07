using System;
using System.Collections.Generic;
using System.Linq;

namespace Comet
{
	public class AnimationSequence<T> : Animation where T : View
	{
		//TODO: Do something for key frames
		public AnimationSequence()
		{

		}

		public AnimationSequence(T view)
		{
			View = view;
		}

		public T View { get; set; }
		List<AnimationStep<T>> steps = new List<AnimationStep<T>>();
		public AnimationSequence<T> Animate(Easing easing, Action<T> action, Action completed = null, double duration = .2, double delay = 0,string id = null, Lerp lerp = null)
		{
			steps.Add(new AnimationStep<T>
			{
				Easing = easing,
				Action = action,
				Completed = completed,
				Duration = duration,
				Delay = delay,
				Id = id,
				Lerp = lerp,
			});
			return this;
		}
		public AnimationSequence<T> Animate(Action<T> action, Action completed = null, double duration = .2, double delay = 0)
			=> Animate(Easing.Default, action, completed, duration, delay);


		public T EndAnimationSequence()
		{
			var duration = steps.Sum(x => x.Delay + x.Duration);
			Duration = duration;
			View.AddAnimation(this);
			return View;
		}

		Animation currentAnimation;
		int currentIndex = 0;
		protected override void OnTick(double secondsSinceLastUpdate)
		{
			CurrentTime += secondsSinceLastUpdate;
			currentAnimation ??= GetNextAnimation();
			if (currentAnimation == null)
			{
				HasFinished = true;
				return;
			}

			currentAnimation.Tick(secondsSinceLastUpdate);
			if (currentAnimation.HasFinished)
			{
				currentAnimation = null;
			}
		}

		Animation GetNextAnimation()
		{
			if (currentIndex >= steps.Count)
			{
				if (!Repeats)
					return null;
				currentIndex = 0;
			}
			var step = steps[currentIndex];
			currentIndex++;
			var animation = AnimationExtensions.CreateAnimation<T>(View, step.Easing, step.Action, step.Completed, step.Duration, step.Delay, id: step.Id, lerp: step.Lerp) ?? GetNextAnimation();
			return animation;
		}

	}

	public class AnimationStep<T>
	{
		public Easing Easing { get; set; }
		public Action<T> Action { get; set; }
		public Action Completed { get; set; }
		public double Duration { get; set; }
		public double Delay { get; set; }
		public string Id { get; set; }
		public Lerp Lerp { get; set; }
		//public bool Repeats { get; set; }
		//public bool AutoReverses { get; set; }

	}
}
