using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Comet.Internal;

namespace Comet
{
	public class Animation : IDisposable
	{
		public Animation()
		{

		}
	
		public Animation(List<Animation> animations)
		{
			childrenAnimations = animations;
		}
		bool paused;
		public bool IsPaused => paused;
		object locker = new object();
		List<Animation> childrenAnimations = new List<Animation>();
		public double StartDelay { get; set; }
		public double Duration { get; set; }
		public double CurrentTime { get; protected set; }
		public Easing Easing { get; set; }
		public bool HasFinished { get; protected set; }
		public object StartValue { get; set; }
		public object EndValue { get; set; }
		public object CurrentValue { get; protected set; }
		public bool Repeats { get; set; }
		public string Id { get; set; }
		public ContextualObject ContextualObject { get; set; }
		public string PropertyName { get; set; }
		public bool PropertyCascades { get; set; }
		Lerp _lerp;
		public Lerp Lerp
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
			set => _lerp = value;
		}
		double skippedSeconds;
		int usingResource = 0;
		public void Tick(double secondsSinceLastUpdate)
		{
			if (IsPaused)
				return;

			if (0 == Interlocked.Exchange(ref usingResource, 1))
			{
				try
				{
					OnTick(skippedSeconds + secondsSinceLastUpdate);
					skippedSeconds = 0;
				}
				finally
				{
					//Release the lock
					Interlocked.Exchange(ref usingResource, 0);
				}
			}
			//animation is lagging behind!
			else
			{
				skippedSeconds += secondsSinceLastUpdate;
			}
		}

		protected virtual void OnTick(double secondsSinceLastUpdate)
		{
			if (HasFinished)
				return;

			CurrentTime += secondsSinceLastUpdate;
			if (childrenAnimations.Any())
			{
				var hasFinished = true;
				foreach (var animation in childrenAnimations)
				{

					animation.OnTick(secondsSinceLastUpdate);
					if (!animation.HasFinished)
						hasFinished = false;

				}
				HasFinished = hasFinished;


			}
			else
			{

				var start = CurrentTime - StartDelay;
				if (CurrentTime < StartDelay)
					return;
				var percent = Math.Min(start / Duration, 1);
				Update(percent);
			}
			if (HasFinished && Repeats)
			{
				Reset();
			}
		}

		public virtual void Update(double percent)
		{
			try
			{
				var progress = Easing.Ease(percent);
				CurrentValue = Lerp.Calculate(StartValue, EndValue, progress);
				ContextualObject?.SetEnvironment(PropertyName, CurrentValue, PropertyCascades);
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
				ContextualObject = ContextualObject,
				PropertyCascades = PropertyCascades,
				PropertyName = PropertyName,
				Lerp = Lerp,
			};
			var parentAnimation = new Animation
			{
				Duration = reveresed.StartDelay + reveresed.Duration,
				Repeats = Repeats,
				childrenAnimations =
				{
					this,
					reveresed,
				}
			};
			Repeats = false;
			return parentAnimation;
		}


		public void Reset()
		{
			CurrentTime = 0;
			HasFinished = false;
			foreach (var x in childrenAnimations)
				x.Reset();
		}

		public void Pause()
        {
			paused = true;
			AnimationManger.Remove(this);
        }
		public void Resume()
        {
			paused = false;
			AnimationManger.Add(this);
        }

		#region IDisposable Support
		public bool IsDisposed => disposedValue;
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					foreach (var child in childrenAnimations)
						child.Dispose();
					childrenAnimations.Clear();
				}
				ContextualObject = null;
				disposedValue = true;
				AnimationManger.Remove(this);
			}
		}


		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
		}
		#endregion
	}
}