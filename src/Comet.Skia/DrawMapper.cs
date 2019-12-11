using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using SkiaSharp;

namespace Comet.Skia
{
	public class DrawMapper<TVirtualView> : Dictionary<string, Action<SKCanvas, RectangleF, SkiaControl, TVirtualView>>
		where TVirtualView : View
	{
		private readonly DrawMapper<View> _chained;

		public DrawMapper()
		{
		}

		public DrawMapper(DrawMapper<View> chained)
		{
			_chained = chained;
		}

		protected bool DrawLayer(string key, SKCanvas canvas, RectangleF dirtyRect, SkiaControl viewHandler, TVirtualView virtualView)
		{
			if (this.TryGetValue(key, out var action))
			{
				action?.Invoke(canvas, dirtyRect, viewHandler, virtualView);
				return true;
			}
			else
				return _chained?.DrawLayer(key, canvas, dirtyRect, viewHandler, virtualView) ?? false;
		}

		public bool DrawLayer(SKCanvas canvas, RectangleF dirtyRect, SkiaControl viewHandler, TVirtualView virtualView, string property)
		{
			if (virtualView == null)
				return false;

			if (TryGetValue(property, out var updater))
			{
				updater.Invoke(canvas, dirtyRect, viewHandler, virtualView);
				return true;
			}

			return _chained?.DrawLayer(canvas, dirtyRect, viewHandler, virtualView, property) ?? false;
		}
	}
}
