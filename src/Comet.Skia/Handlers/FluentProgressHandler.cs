using System;
using System.Collections.Generic;
using System.Drawing;

namespace Comet.Skia
{
	public partial class FluentProgressHandler : SkiaAbstractControlHandler<ProgressBar>
	{

		public static new readonly PropertyMapper<ProgressBar> Mapper = new PropertyMapper<ProgressBar>(SkiaControl.Mapper)
		{
			[nameof(ProgressBar.Value)] = MapSetProgress,
		};

		static void MapSetProgress(IViewHandler viewHandler, ProgressBar virtualView)
		{
			(viewHandler as FluentProgressHandler)?.SetProgress();
		}
		bool hasSet = false;
		protected void SetProgress()
		{
			if (!hasSet)
			{
				SetDefaultValues();
				hasSet = true;
			}
			SetRect();

		}
		void SetRect()
		{
			//PRogress
			var rect = this.GetEnvironment<RectangleF?>(progressRectKey) ?? this.Bounds;
			rect.Width = this.TypedVirtualView.Value.CurrentValue * Bounds.Width;
			this.SetEnvironment(progressRectKey, rect);

			//BackgroundColor
			rect = this.GetEnvironment<RectangleF?>(backgroundRectKey) ?? this.Bounds;
			rect.Width = Bounds.Width;
			this.SetEnvironment(backgroundRectKey, rect);
		}
		public override void Resized(RectangleF bounds)
		{
			base.Resized(bounds);
			SetRect();
		}

		protected void SetDefaultValues()
		{
			foreach (var key in stateKeys)
			{
				var value = GetValueForState<object>(key);
				this.SetEnvironment(key, value);
			}
		}

		static Dictionary<string, string> variableTransforms = new Dictionary<string, string>
		{
			["🎨 ProgressFill"] = EnvironmentKeys.ProgressBar.ProgressColor,
			["🎨 BackgroundRectFill"] = EnvironmentKeys.ProgressBar.TrackColor,
		};

		public override T GetValueForState<T>(string key)
		{
			if (variableTransforms.TryGetValue(key, out var newKey))
			{
				var defaultValue = TypedVirtualView.GetEnvironment<T>(newKey);
				if (defaultValue != null)
					return defaultValue;
			}
			if (stateDefaultValues.TryGetValue(key, out var r) && r is T t)
				return t;
			return default;
		}

		public string progressRectKey = "🎨 ProgressRect";
		public string backgroundRectKey = "🎨 BackgroundRectRect";

		public override string AccessibilityText() => (TypedVirtualView?.Value?.CurrentValue ?? 0).ToString("P");
	}
}
