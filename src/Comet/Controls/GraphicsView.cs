using System;
using Microsoft.Maui.Graphics;

namespace Comet
{
	public class GraphicsView : View, IDrawable
	{
		public Action<ICanvas, RectF> Draw { get; set; }
		void IDrawable.Draw(ICanvas canvas, RectF dirtyRect) {
			//TODO: Remove this later, it's a temp hack for Android
			if (canvas.DisplayScale > 1)
			{
				dirtyRect.Width /= canvas.DisplayScale;
				dirtyRect.Height /= canvas.DisplayScale;
			}
			Draw?.Invoke(canvas, dirtyRect);
		}
	}
}
