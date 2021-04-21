using System.Collections.Generic;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using GHorizontalAlignment = Microsoft.Maui.Graphics.HorizontalAlignment;
using GVerticalAlignment = Microsoft.Maui.Graphics.VerticalAlignment;

namespace Comet.GraphicsControls
{
	public class FluentSliderDrawable : ViewDrawable<ISlider>, ISliderDrawable
	{

		static readonly Dictionary<string, object> stateDefaultValues = new Dictionary<string, object>
		{
			["TextSize"] = 36f
		};

		RectangleF trackRect = new RectangleF();
		public RectangleF TrackRect => trackRect;

		RectangleF touchTargetRect = new RectangleF(0, 0, 44, 44);
		public RectangleF TouchTargetRect => touchTargetRect;

		public override void DrawBackground(ICanvas canvas, RectangleF dirtyRect, IView view) 
		{
			canvas.SaveState();

			var slider = VirtualView;

			canvas.FillColor = slider.MaximumTrackColor;

			var x = dirtyRect.X;

			stateDefaultValues.TryGetValue("TextSize", out var textSize);
			var width = dirtyRect.Width - (float)textSize;
			var height = 4;

			var y = (float)((dirtyRect.Height - height) / 2);

			trackRect.X = x;
			trackRect.Width = width;

			canvas.FillRoundedRectangle(x, y, width, height, 0);

			canvas.RestoreState();
		}

		public virtual void DrawTrackProgress(ICanvas canvas, RectangleF dirtyRect, ISlider view)
		{
			canvas.SaveState();

			var slider = VirtualView;


			canvas.FillColor = slider.MinimumTrackColor;

			var x = dirtyRect.X;

			var value = ((double)slider.Value).Clamp(0, 1);
			stateDefaultValues.TryGetValue("TextSize", out var textSize);
			var width = (float)((dirtyRect.Width - (float)textSize) * value);

			var height = 4;

			var y = (float)((dirtyRect.Height - height) / 2);

			canvas.FillRoundedRectangle(x, y, width, height, 0);

			canvas.RestoreState();
		}

		public virtual void DrawThumb(ICanvas canvas, RectangleF dirtyRect, ISlider view)
		{
			canvas.SaveState();

			var slider = VirtualView;

			var size = 16f;
			var strokeWidth = 2f;

			canvas.StrokeColor = slider.ThumbColor;

			canvas.StrokeSize = strokeWidth;

			var value = ((double)slider.Value).Clamp(0, 1);
			stateDefaultValues.TryGetValue("TextSize", out var textSize);
			var x = (float)(((dirtyRect.Width - (float)textSize) * value) - (size / 2));

			if (x <= strokeWidth)
				x = strokeWidth;

			if (x >= dirtyRect.Width - (size + strokeWidth))
				x = dirtyRect.Width - (size + strokeWidth);

			var y = (float)((dirtyRect.Height - size) / 2);

			touchTargetRect.Center(new PointF(x, y));

			canvas.FillColor = Colors.Black;

			canvas.FillEllipse(x, y, size, size);
			canvas.DrawEllipse(x, y, size, size);

			canvas.RestoreState();
		}

		public virtual void DrawText(ICanvas canvas, RectangleF dirtyRect, ISlider text)
		{
			canvas.SaveState();

			var slider = VirtualView;

			canvas.FontColor = Colors.Black;
			canvas.FontSize = 14f;

			var height = dirtyRect.Height;
			var width = dirtyRect.Width;

			var margin = 6;
			stateDefaultValues.TryGetValue("TextSize", out var textSize);
			var x = (float)(width - (float)textSize + margin);
			var y = 2;

			canvas.SetToBoldSystemFont();

			var value = ((double)slider.Value).Clamp(0, 1).ToString("####0.00");

			canvas.DrawString(value, x, y, (float)textSize, height, GHorizontalAlignment.Left, GVerticalAlignment.Center);

			canvas.RestoreState();
		}

		public override Size GetDesiredSize(IView view, double widthConstraint, double heightConstraint) => new Size(widthConstraint,20f);

	}
}