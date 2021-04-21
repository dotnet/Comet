using System;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace Comet.GraphicsControls
{
	public interface ISliderDrawable : IViewDrawable<ISlider>
	{
		RectangleF TrackRect { get; }
		RectangleF TouchTargetRect { get; }
		void DrawThumb(ICanvas canvas, RectangleF dirtyRect, ISlider view);
		void DrawTrackProgress(ICanvas canvas, RectangleF dirtyRect, ISlider view);
		void DrawText(ICanvas canvas, RectangleF dirtyRect, ISlider view);
	}
}
