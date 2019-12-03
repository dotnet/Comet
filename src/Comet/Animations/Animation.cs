using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Comet.Internal;

namespace Comet
{
	public class Animation
	{
		List<Animation> childrenAnimations = new List<Animation>();
		public double StartDelay { get; set; } = 0;
		public double Duration { get; set; }
		public double CurrentTime { get; private set; }
		public Easing Easing { get; set; }
		public bool HasFinished { get; private set; }
		public object StartValue { get; set; }
		public object EndValue { get; set; }
		public object CurrentValue { get; private set; }
		public Action<object> ValueChanged { get; set; }
		Lerp _lerp;
		Lerp Lerp
		{
			get
			{
				if (_lerp != null)
					return _lerp;

				//TODO: later we should find the first matching types of the subclasses
				var type = StartValue?.GetType() ?? EndValue?.GetType();
				if (type == null)
					return null;
				return _lerp = Lerp.GetLerp(type);
			}
		}
		public void Tick(double secondsSinceLastUpdate)
        {
			if (HasFinished)
				return;
			CurrentTime += secondsSinceLastUpdate;
			if (childrenAnimations.Any())
			{
				var hasFinished = true;
				foreach(var animation in childrenAnimations)
                {

					animation.Tick(secondsSinceLastUpdate);
					if (!animation.HasFinished)
						hasFinished = false;

				}
				HasFinished = hasFinished;
			
				if (HasFinished)
					Console.WriteLine("Hi");
			}
			else
			{

				var start = CurrentTime - StartDelay;
				Console.WriteLine($"{start} = {CurrentTime} - {StartDelay} : {CurrentTime < StartDelay}");
				if (CurrentTime < StartDelay)
					return;
				var percent = Math.Min(start / Duration, 1);
				Update(percent);
			}
		}

		public void Update(double percent)
		{
			try
			{
				var progress = Easing.Ease(percent);
				CurrentValue = Lerp.Calculate(progress, StartValue, EndValue);
				ValueChanged?.Invoke(CurrentValue);
				HasFinished = percent == 1;
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
				CurrentValue = EndValue;
				HasFinished = true;
			}
		}
		public Animation CreateAutoReversing()
        {
			var reveresedChildren = childrenAnimations.ToList();
			reveresedChildren.Reverse();
			var reveresed = new Animation
			{
				Easing = Easing,
				Duration = Duration,
				StartDelay = StartDelay + Duration,
				StartValue = EndValue,
				EndValue = StartValue,
				childrenAnimations = reveresedChildren,
				ValueChanged = ValueChanged,
			};
			return new Animation
			{
				Duration = reveresed.StartDelay + reveresed.Duration,
				childrenAnimations =
				{
					this,
					reveresed,
				}
			};
        }
	}
}