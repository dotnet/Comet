using System;
using System.Collections.Generic;

namespace HotUI
{
    public abstract class ContextualObject
    {
        internal static readonly EnvironmentData Environment = new EnvironmentData();
        internal EnvironmentData _context;
        internal EnvironmentData Context(bool shouldCreate) => _context ?? (shouldCreate ? (_context = new EnvironmentData(this)) : null);



        protected ContextualObject()
        {

        }

        internal abstract void ContextPropertyChanged(string property, object value);

        public object GetValue(string key, View view)
        {
            try
            {
                var value =  Context(false)?.GetValueInternal(key);
                if (value == null)
                {
                    if (view == null)
                        return View.Environment.GetValueInternal(key);
                    value = view.GetValue(key,view.Parent);
                }
                return value;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public T GetValue<T>(string key, View view)
        {
            try
            {
                var value = GetValue(key, view);
                return (T)value;
            }
            catch (Exception ex)
            {
                return default;
            }
        }

        public void SetValue(string key, object value)
        {
            Context(true).SetValue(key, value);
        }
        
        //protected ICollection<string> GetAllKeys()
        //{
        //    //This is the global Environment
        //    if (View?.Parent == null)
        //        return dictionary.Keys;

        //    //TODO: we need a fancy way of collapsing this. This may be too slow
        //    var keys = new HashSet<string>();
        //    var localKeys = dictionary?.Keys;
        //    if (localKeys != null)
        //        foreach (var k in localKeys)
        //            keys.Add(k);

        //    var parentKeys = View?.Parent?.Context?.GetAllKeys() ?? View.Environment.GetAllKeys();
        //    if (parentKeys != null)
        //        foreach (var k in parentKeys)
        //            keys.Add(k);
        //    return keys;
        //}
    }

    public static class ContextualObjectExtensions
    {
        public static T SetEnvironment<T>(this T contextualObject, string key, object value) where T : ContextualObject
        {
            contextualObject.SetValue(key, value);
            Device.InvokeOnMainThread(() => {
                contextualObject.ContextPropertyChanged(key, value);
            });
            return contextualObject;
        }

        public static T SetEnvironment<T>(this T contextualObject, IDictionary<string, object> data) where T : ContextualObject
        {
            foreach (var pair in data)
                contextualObject.SetValue(pair.Key, pair.Value);

            return contextualObject;
        }
        public static T GetEnvironment<T>(this ContextualObject contextualObject, View view, string key) => contextualObject.GetValue<T>(key, view);
        public static object GetEnvironment(this ContextualObject contextualObject, View view, string key) => contextualObject.GetValue(key, view);
        public static T GetEnvironment<T>(this View view, string key) => view.GetValue<T>(key, view.Parent);
        public static object GetEnvironment(this View view, string key) => view.GetValue(key, view.Parent);
    }
}
