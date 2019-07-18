using System.Collections.Generic;

namespace HotUI
{
    public abstract class ContextualObject
    {
        internal static readonly EnvironmentData Environment = new EnvironmentData();
        internal readonly EnvironmentData Context;

        protected ContextualObject()
        {
            Context = new EnvironmentData(this);
        }

        internal abstract void ContextPropertyChanged(string property, object value);
    }

    public static class ContextualObjectExtensions
    {
        public static T SetEnvironment<T>(this T contextualObject, string key, object value) where T : ContextualObject
        {
            contextualObject.Context.SetValue(key, value);
            Device.InvokeOnMainThread(() => {
                contextualObject.ContextPropertyChanged(key, value);
            });
            return contextualObject;
        }

        public static T SetEnvironment<T>(this T contextualObject, IDictionary<string, object> data) where T : ContextualObject
        {
            foreach (var pair in data)
                contextualObject.Context.SetValue(pair.Key, pair.Value);

            return contextualObject;
        }

        public static T GetEnvironment<T>(this ContextualObject contextualObject, string key) => contextualObject.Context.GetValue<T>(key);
        public static object GetEnvironment(this ContextualObject contextualObject, string key) => contextualObject.Context.GetValue(key);
    }
}