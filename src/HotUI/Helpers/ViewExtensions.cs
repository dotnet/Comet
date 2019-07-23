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
                return ((ContentView)view).Content.GetViewWithTag(tag);

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

        public static T AddGesture<T>(this T view, Gesture gesture) where T : View
        {
            view.gestures.Add(gesture);
            view?.ViewHandler?.UpdateValue(HotUI.Gesture.AddGestureProperty, gesture);
            return view;
        }
        public static T RemoveGesture<T>(this T view, Gesture gesture) where T : View
        {
            view.gestures.Remove(gesture);
            view?.ViewHandler?.UpdateValue(HotUI.Gesture.RemoveGestureProperty, gesture);
            return view;
        }

        public static T OnTap<T>(this T view, Action<T> action) where T: View
            => view.AddGesture(new TapGesture((g)=> action?.Invoke(view)));

        public static T Navigate<T>(this T view, Func<View> destination) where T : View
            => view.OnTap((v) => NavigationView.Navigate(view, destination.Invoke()));

        public static ListView<T> OnSelectedNavigate<T>(this ListView<T> view, Func<T, View> destination)
        {
            return view.OnSelected(v => NavigationView.Navigate(view, destination?.Invoke(v)));
        }
    }
}