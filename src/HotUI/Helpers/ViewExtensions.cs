using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HotUI
{
    public static class ViewExtensions
    {
        public static View GetViewWithTag(this View view, string tag)
        {
            if (view == null) return null;

            if (view.Tag == tag)
                return view;

            if (view is AbstractLayout layout)
            {
                foreach (var subView in layout)
                {
                    var match = subView.GetViewWithTag(tag);
                    if (match != null)
                        return match;
                }
            }

            if (view.GetType() == typeof(ContentView))
                return ((ContentView) view).Content.GetViewWithTag(tag);

            return view.BuiltView.GetViewWithTag(tag);
        }

        public static T GetViewWithTag<T>(this View view, string tag) where T : View
        {
            return view.GetViewWithTag(tag) as T;
        }

        public static T Tag<T>(this T view, string tag) where T : View
        {
            view.Tag = tag;
            return view;
        }

        public static ListView<T> OnSelected<T>(this ListView<T> listview, Action<T> selected)
        {
            listview.ItemSelected = (o) => 
            {
                selected?.Invoke((T)o); 
            };
            return listview;
        }

        public static List<FieldInfo> GetFieldsWithAttribute(this object obj, Type attribute)
        {
            var type = obj.GetType();
            var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).Where(x => Attribute.IsDefined(x, attribute)).ToList();
            return fields;
        }

        public static T Title<T>(this T view, string title) where T : View =>
            view.SetEnvironment(EnvironmentKeys.View.Title, title);
    }
}