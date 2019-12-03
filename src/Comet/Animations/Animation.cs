using System;
using System.Diagnostics;

namespace Comet
{
	public class Animation
	{
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
			CurrentTime += secondsSinceLastUpdate;
			var start = CurrentTime - StartDelay;
			var percent = Math.Min(start / Duration, 1);
			Update(percent);
			HasFinished = percent == 1;
		}

		public void Update(double percent)
		{
			try
			{
				var progress = Easing.Ease(percent);
				CurrentValue = Lerp.Calculate(progress, StartValue, EndValue);
				//ThreadHelper.RunOnMainThread(() =>
				ValueChanged?.Invoke(CurrentValue);
				//);
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
				CurrentValue = EndValue;
				HasFinished = true;
			}
		}
	}
}