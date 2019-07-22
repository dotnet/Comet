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
		internal Dictionary<Type, Type> Handler = new Dictionary<Type, Type> ();

		public void Register<TView, TRender> ()
			where TView : TType
				where TRender : TTypeRender
		{
            Register(typeof(TView), typeof(TRender));
		}

        public void Register(Type view, Type handler)
        {
            Handler[view] = handler;
        }
        public TTypeRender GetRenderer<T> ()
		{
			return GetRenderer (typeof (T));
		}

        internal List<Type> GetViewType(Type type) => Handler.Where(x => x.Value == type).Select(x=> x.Key).ToList();


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

        public Type GetRendererType(Type type)
        {
            List<Type> types = new List<Type> { type };
            Type baseType = type.BaseType;
            while (baseType != null)
            {
                types.Add(baseType);
                baseType = baseType.BaseType;
            }

            foreach (var t in types)
            {
                if (Handler.TryGetValue(t, out var returnType))
                    return returnType;
            }
            return null;
        }

        TTypeRender getRenderer (Type t)
		{
            if(!Handler.TryGetValue(t, out var renderer))
				return default (TTypeRender);
			try {
				var newObject = Activator.CreateInstance (renderer);
				return (TTypeRender)newObject;
			} catch (Exception ex) {
				if (Debugger.IsAttached)
					throw ex;
			}
			
			return default;
		}
	}
}