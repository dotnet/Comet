using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace Comet.GraphicsControls
{
	public class DrawMapper
	{
		internal Dictionary<string, Action<ICanvas, RectangleF, IViewDrawable, IView>> genericMap = new Dictionary<string, Action<ICanvas, RectangleF, IViewDrawable, IView>>();

		protected bool DrawLayer(string key, ICanvas canvas, RectangleF dirtyRect, IViewDrawable drawable, IView virtualView)
		{
			var action = Get(key);
			if (action == null)
				return false;
			action.Invoke(canvas, dirtyRect, drawable, virtualView);
			return true;
		}
		public bool DrawLayer(ICanvas canvas, RectangleF dirtyRect, IViewDrawable drawable, IView virtualView, string property)
		{
			if (virtualView == null)
				return false;
			return DrawLayer(property, canvas,dirtyRect,drawable, virtualView);
		}
		

		public virtual Action<ICanvas, RectangleF, IViewDrawable, IView> Get(string key)
		{
			genericMap.TryGetValue(key, out var action);
			return action;
		}
	}




	public class DrawMapper<TViewDrawable, TVirtualView> : DrawMapper
		where TVirtualView : IView
		where TViewDrawable : IViewDrawable
	{

		public DrawMapper()
		{

		}

		public DrawMapper(DrawMapper chained)
		{
			Chained = chained;
		}

		public DrawMapper Chained { get; set; }

		public Action<ICanvas, RectangleF, TViewDrawable, TVirtualView> this[string key]
		{
			set => genericMap[key] = (c,r,h, v) => value?.Invoke(c,r,(TViewDrawable)h, (TVirtualView)v);
		}

		public override Action<ICanvas, RectangleF, IViewDrawable, IView> Get(string key)
		{
			if (genericMap.TryGetValue(key, out var action))
				return action;
			else
				return Chained?.Get(key) ?? null;
		}
	}
}
