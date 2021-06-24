using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Comet.Internal;
using Microsoft.Maui;
using Microsoft.Maui.Animations;

namespace Comet
{
	public class ContextualAnimation : LerpingAnimation
	{
		public ContextualAnimation()
		{
		}

		public ContextualAnimation(Action<double> callback, double start = 0, double end = 1, Easing easing = null, Action finished = null) : base(callback, start, end, easing, finished)
		{
		}

		public ContextualAnimation(List<Animation> animations) : base(animations)
		{
		}
		public string Id { get; set; }
		public ContextualObject ContextualObject { get; set; }
		public string PropertyName { get; set; }
		public bool PropertyCascades { get; set; }
		

		public override void Update(double percent)
		{
			try
			{
				base.Update(percent);
				var oldV = ContextualObject?.GetEnvironment(Parent.GetValueOfType<View>(), PropertyName, PropertyCascades);
				if (oldV is Binding b)
					b.BindingValueChanged(null, PropertyName, CurrentValue);
				else
					ContextualObject?.SetEnvironment(PropertyName, CurrentValue, PropertyCascades);
			}
			catch (Exception ex)
			{
				var message = $"Error lerping: {StartValue} to {EndValue} on {PropertyName}";
				Debug.WriteLine(message);
				Logger.Error(ex, message);
				CurrentValue = EndValue;
				HasFinished = true;
			}
		}
		protected override Animation CreateReverse() => new ContextualAnimation
			{
				Easing = Easing,
				Duration = Duration,
				StartDelay = StartDelay + Duration,
				StartValue = EndValue,
				EndValue = StartValue,
				ContextualObject = ContextualObject,
				PropertyCascades = PropertyCascades,
				PropertyName = PropertyName,
				Lerp = Lerp,
			};
}
}