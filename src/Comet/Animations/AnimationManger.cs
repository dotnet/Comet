using System;
using System.Collections.Generic;
using System.Linq;

namespace Comet
{
	public static class AnimationManger
	{
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
			if(!Ticker.SystemEnabled)
            {
				return;
            }
			Animations.Add(animation);
			if(!Ticker.IsRunning)
				Start();
        }

		static void Start()
		{
			lastUpdate = GetCurrentTick();
			Ticker.Start();
		}

		static long GetCurrentTick() => (Environment.TickCount & Int32.MaxValue);

		static void End() => Ticker.Stop();

		static void OnFire()
		{
			var now = GetCurrentTick();
			var seconds = TimeSpan.FromMilliseconds((now - lastUpdate)).TotalSeconds;
			lastUpdate = now;
			var animations = Animations.ToList();
			//TODO: do this in parallel
			foreach (var animation in animations)
			{
				animation.Tick(seconds);
				if (animation.HasFinished)
				{
					Animations.Remove(animation);
					animation.Dispose();
				}
			}

			if (!Animations.Any())
				End();
		}

	}
}
