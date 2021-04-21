using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace Comet
{
	public class DrawMapper
	{
		internal Dictionary<string, Action<ICanvas, RectangleF, IViewHandler, IView>> genericMap = new Dictionary<string, Action<ICanvas, RectangleF, IViewHandler, IView>>();

		protected bool DrawLayer(string key, ICanvas canvas, RectangleF dirtyRect, IViewHandler viewHandler, IView virtualView)
		{
			var action = Get(key);
			if (action == null)
				return false;
			action.Invoke(canvas, dirtyRect, viewHandler, virtualView);
			return true;
		}
		public bool DrawLayer(ICanvas canvas, RectangleF dirtyRect, IViewHandler viewHandler, IView virtualView, string property)
		{
			if (virtualView == null)
				return false;
			return DrawLayer(property, canvas,dirtyRect,viewHandler, virtualView);
		}
		

		public virtual Action<ICanvas, RectangleF, IViewHandler, IView> Get(string key)
		{
			genericMap.TryGetValue(key, out var action);
			return action;
		}
	}




	public class DrawMapper<TViewHandler, TVirtualView> : DrawMapper
		where TVirtualView : IView
		where TViewHandler : IViewHandler
	{
		private readonly DrawMapper _chained;

		public DrawMapper()
		{
		}

		public DrawMapper(DrawMapper chained)
		{
			Chained = chained;
		}
		public DrawMapper Chained { get; set; }



		public Action<ICanvas, RectangleF, TViewHandler, TVirtualView> this[string key]
		{
			set => genericMap[key] = (c,r,h, v) => value?.Invoke(c,r,(TViewHandler)h, (TVirtualView)v);
		}

		public override Action<ICanvas, RectangleF, IViewHandler, IView> Get(string key)
		{
			if (genericMap.TryGetValue(key, out var action))
				return action;
			else
				return Chained?.Get(key) ?? null;
		}
	}
}
