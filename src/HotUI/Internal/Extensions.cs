using System;
using System.Collections.Generic;
using System.Linq;
using HotUI.Reflection;

namespace HotUI.Internal
{
    public static class Extensions
    {
        public static Func<View> GetBody(this View view)
        {
            var bodyMethod = view.GetType().GetDeepMethodInfo(typeof(BodyAttribute));
            if (bodyMethod != null)
                return (Func<View>)Delegate.CreateDelegate(typeof(Func<View>), view, bodyMethod.Name);
            return null;
        }

        public static void ResetGlobalEnvironment(this View view) => View.Environment.Clear();

        public static void DisposeAllViews(this View view) => View.ActiveViews.Clear();

        public static View GetView(this View view) => view.GetView();

        public static void UpdateFromOldView(this View view, View newView) => view.UpdateFromOldView(newView);

        public static Dictionary<Type, Type> GetAllRenderers(this Registrar<View, IViewHandler> registar) => registar.Handler;
       
    }
}
