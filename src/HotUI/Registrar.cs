using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
namespace HotUI {
	public static class Registrar {
		public static Registrar<View, IViewHandler> Handlers { get; private set; }
		static Registrar ()
		{
			Handlers = new Registrar<View, IViewHandler> ();
		}
	}
	public class Registrar<TType, TTypeRender> {
		internal Dictionary<Type, List<Type>> Handler = new Dictionary<Type, List<Type>> ();

		public void Register<TView, TRender> ()
			where TView : TType
				where TRender : TTypeRender
		{
			List<Type> renderers;
			if (!Handler.TryGetValue (typeof (TView), out renderers)) {
				renderers = new List<Type> { typeof (TRender) };
				Handler.Add (typeof (TView), renderers);
				return;
			}
			renderers.Add (typeof (TRender));

		}
		public TTypeRender GetRenderer<T> ()
		{
			return GetRenderer (typeof (T));
		}
		public TTypeRender GetRenderer (Type type)
		{
			List<Type> types = new List<Type> { type };
			Type baseType = type.BaseType;
			while (baseType != null) {
				types.Add (baseType);
				baseType = baseType.BaseType;
			}

			foreach (var t in types) {
				var renderer = getRenderer (t);
				if (renderer != null)
					return renderer;
			}
			return default (TTypeRender);
		}
		TTypeRender getRenderer (Type t)
		{
			if (!Handler.ContainsKey (t))
				return default (TTypeRender);
			var renderers = Handler [t];

			var length = renderers.Count;

			for (var i = 0; i < length; i++) {
				var index = length - (i + 1);
				var renderer = renderers [index];
				try {
					var newObject = Activator.CreateInstance (renderer);
					return (TTypeRender)newObject;
				} catch (Exception ex) {
					if (Debugger.IsAttached)
						throw ex;
				}
			}
			return default (TTypeRender);
		}
	}
}