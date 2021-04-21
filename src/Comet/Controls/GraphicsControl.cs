using System;
using Microsoft.Maui.Graphics;
namespace Comet
{
	public class GraphicsControl : View, IDrawable
	{
		public GraphicsControl()
		{
		}

		void IDrawable.Draw(ICanvas canvas, RectangleF dirtyRect) => throw new NotImplementedException();
	}
}
