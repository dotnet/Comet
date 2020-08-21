using System;
using System.Drawing;

namespace Comet.Skia
{
	public partial class FluentButtonHandler : SkiaAbstractControlHandler<Button>
	{
		public static new readonly PropertyMapper<Button> Mapper = new PropertyMapper<Button>(SkiaControl.Mapper)
		{
			[nameof(Button.Text)] = MapSetup,
			[EnvironmentKeys.Colors.Color] = MapSetup
		};
		
		static void MapSetup(IViewHandler viewHandler, View virtualView)
		{
			var v = (viewHandler as FluentButtonHandler);
			v?.SetState();
		}
		bool hasSet = false;
		protected void SetState()
		{
			if (!hasSet)
			{
				hasSet = true;
				SetValues();
			}
			this.Animate((t) => {
				SetValues();
			});

		}
		protected override void ControlStateChanged()
		{
			base.ControlStateChanged();
			SetState();
		}
		protected void SetValues()
		{
			foreach (var key in stateKeys)
			{
				var value = GetValueForState<object>(key);
				this.SetEnvironment(key, value);
			}
		}

		static string textKey => "Default.Content✏️ Label";

		public override T GetValueForState<T>(string key)
		{
			if (key == textKey)
				return (T)(object)TypedVirtualView.Text; 
			var state = CurrentState switch
			{
				ControlState.Pressed => "Pressed",
				ControlState.Disabled => "Disabled",
				_ => "Default",
			};
			//var isOnText = currentState ? "On" : "Off";
			//var isDisabled = state == ControlState.Disabled ? " (disabled)" : "";
			var newKey = $"{state}.{key}";
			if (stateDefaultValues.TryGetValue(newKey, out var r) && r is T t)
				return t;
			return default;
		}

		public override void EndInteraction(PointF[] points, bool contained)
		{
			if (contained)
				TypedVirtualView?.OnClick?.Invoke();
			base.EndInteraction(points, contained);
		}

		public override string AccessibilityText() => TypedVirtualView?.Text;
	}
}
