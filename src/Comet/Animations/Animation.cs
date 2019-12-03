using System;

namespace Comet
{
	public class Animation
	{
		public double Start { get; set; } = 0;
		public double Stop { get; set; }
		public double Duration { get; set; }
		public Easing Easing { get; set; }
		public bool HasFinished { get; set; }
		public object StartValue { get; set; }
		public object EndValue { get; set; }
		public object CurrentValue { get; set; }
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
		public void Tick(double percent)
		{
			try
			{
				var progress = Easing.Ease(percent);
				CurrentValue = Lerp.Calculate(progress, StartValue, EndValue);
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