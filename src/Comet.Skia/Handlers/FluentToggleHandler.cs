using System;
using System.Drawing;
using System.Linq;
using Comet.Internal;
using SkiaSharp;

namespace Comet.Skia
{
	public partial class FluentToggleHandler : SkiaAbstractControlHandler<Toggle>
	{
		public static new readonly PropertyMapper<Toggle> Mapper = new PropertyMapper<Toggle>(SkiaControl.Mapper)
		{
			[nameof(Toggle.IsOn)] = MapIsOnProperty,
		};

		public static void MapIsOnProperty(IViewHandler viewHandler, Toggle virtualView)
		{
			
			var control = viewHandler as FluentToggleHandler;
			control.SetState(virtualView?.IsOn?.CurrentValue ?? false);
		}

		public override string AccessibilityText() => $"{TypedVirtualView.IsOn?.CurrentValue ?? false}";

		public object GetValueForState (string key, ControlState state, bool isOn)
		{
			
			var isOnText = currentState ? "On" : "Off";
			var isDisabled = state == ControlState.Disabled ? " (disabled)" : "";
			var newKey = $"{isOnText}{isDisabled}.{key}";
			if (stateDefaultValues.TryGetValue(newKey, out var r))
				return r;
			return null;
		}

		public override void EndInteraction(PointF[] points, bool inside)
		{
			if (inside)
				ToggleValue();
			//this.Animate(t => {
			//	t.SetEnvironment(thumbProgressAnimation, 0f);
			//});
			base.EndInteraction(points, inside);
		}

		void ToggleValue()
		{
			//TODO: Change this to a method on Toggle
			var binding = TypedVirtualView?.IsOn;
			if (binding != null)
				binding.Set(!currentState);

			SetState(!currentState);
		}

		bool hasSet = false;
		bool currentState;
		public void SetState(bool state)
		{
			currentState = state;
			if (!hasSet)
			{
				hasSet = true;
				SetVariables(state);
			}
			this.Animate((t) => {
				SetVariables(state);
			});

		}

		void SetVariables(bool state)
		{
			foreach(var key in stateKeys)
			{
				this.SetEnvironment(key, GetValueForState(key, this.CurrentState, state));
			}
		}

		public override SizeF GetIntrinsicSize(SizeF availableSize) => new SizeF(51, 31);


	}
}
