using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comet.Internal;

namespace Comet
{
	public static class AnimationManger
	{
		public static double SpeedModifier { get; set; } = 1;
		static AnimationManger()
		{
			SetTicker(new Ticker());
		}
		static long lastUpdate = GetCurrentTick();
		public static void SetTicker(Ticker ticker)
		{
			var isRunning = Ticker?.IsRunning ?? false;
			Ticker = ticker;
			ticker.Fire = OnFire;
			if (isRunning)
				ticker.Start();
		}

		static Ticker Ticker;

		static List<Animation> Animations = new List<Animation>();
		public static void Add(Animation animation)
		{
			//If animations are disabled, don't do anything
			if (!Ticker.SystemEnabled)
			{
				return;
			}
			if(!Animations.Contains(animation))
				Animations.Add(animation);
			if (!Ticker.IsRunning)
				Start();
		}

		public static void Remove(Animation animation)
        {
			Animations.TryRemove(animation);
			if (!Animations.Any())
				End();
		}

		static void Start()
		{
			lastUpdate = GetCurrentTick();
			Ticker.Start();
		}

		static long GetCurrentTick() => (Environment.TickCount & Int32.MaxValue);

		static void End() => Ticker?.Stop();
		static void OnFire()
		{
			var now = GetCurrentTick();
			var seconds = TimeSpan.FromMilliseconds((now - lastUpdate)).TotalSeconds;
			lastUpdate = now;
			var animations = Animations.ToList();
			Parallel.ForEach(animations, (animation) => {
				if (animation.HasFinished)
				{
					Animations.TryRemove(animation);
					animation.RemoveFromParent();
					return;
				}

				animation.Tick(seconds * SpeedModifier);
				if (animation.HasFinished)
				{
					Animations.TryRemove(animation);
					animation.RemoveFromParent();
				}
			});

			if (!Animations.Any())
				End();
		}

	}
}
