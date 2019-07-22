using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HotUI.Reflection
{
    public static class ReflectionExtensions
    {
        public static bool SetPropertyValue(this object obj, string name, object value)
        {
            var type = obj.GetType();
            var info = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (info != null)
            {

                info.SetValue(obj, Convert(value,info.PropertyType));
                return true;
            }
            else
            {

                var field = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (field == null)
                    return false;
                field.SetValue(obj, Convert(value, field.FieldType));
                return true;
            }
        }

        static object Convert(object obj, Type type)
        {
            if (obj == null)
                return null;
            var newType = obj.GetType();
            if (type.IsAssignableFrom(newType))
                return obj;
            //if (type == typeof(String))
            //    return obj.ToString();
            return System.Convert.ChangeType(obj, type);
        }

        public static bool SetDeepPropertyValue(this object obj, string name, object value)
        {
            if (obj == null)
                return false;
            var lastObect = obj;
            FieldInfo field = null;
            PropertyInfo info = null;
            foreach (var part in name.Split('.'))
            {
                info = null;
                field = null;
                var type = obj.GetType();
                lastObect = obj;
                info = type.GetDeepProperty(part);
                if (info != null)
                {
                    obj = info.GetValue(obj, null);
                }
                else
                {
                    field = type.GetDeepField(part);
                    if (field == null)
                        return false;
                    obj = field.GetValue(obj);
                }
            }
            if (field != null)
            {
                field.SetValue(lastObect, value);
                return true;
            }
            else if (info != null)
            {
                info.SetValue(lastObect, value);
                return true;
            }
            return false;
        }

        public static FieldInfo GetDeepField(this Type type, string name)
        {
            var fieldInfo = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (fieldInfo == null && type.BaseType != null)
                fieldInfo = GetDeepField(type.BaseType, name);
            return fieldInfo;
        }

        public static PropertyInfo GetDeepProperty(this Type type, string name)
        {
            var prop = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (prop == null && type.BaseType != null)
                prop = GetDeepProperty(type.BaseType, name);
            return prop;
        }
        public static List<PropertyInfo> GetDeepProperties(this Type type, BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
        {
            var properties = type.GetProperties(flags).ToList();
            if (type.BaseType != null)
                properties.AddRange(GetDeepProperties(type.BaseType,flags));
            return properties;
        }

        public static MethodInfo GetDeepMethodInfo(this Type type, string name)
        {
            var methodInfo = type.GetMethod(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (methodInfo == null && type.BaseType != null)
                methodInfo = GetDeepMethodInfo(type.BaseType, name);
            return methodInfo;
        }
        public static MethodInfo GetDeepMethodInfo(this Type type, Type withAttribute)
        {
            var methodInfo = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).Where(m => m.GetCustomAttributes(withAttribute, false).Length > 0).FirstOrDefault();
            if (methodInfo == null && type.BaseType != null)
                methodInfo = GetDeepMethodInfo(type.BaseType, withAttribute);
            return methodInfo;
        }

        public static object GetPropertyValue(this object obj, string name)
        {
            foreach (var part in name.Split('.'))
            {
                if (obj == null)
                    return null;
                if (obj is BindingObject bo)
                {
                    obj = bo.GetValueInternal(part);
                }
                else
                {
                    var type = obj.GetType();
                    var info = type.GetProperty(part, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                    if (info != null)
                    {
                        obj = info.GetValue(obj, null);
                    }
                    else
                    {
                        var field = type.GetField(part, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                        if (field == null)
                            return null;
                        obj = field.GetValue(obj);
                    }
                }
            }
            return obj;
        }

        public static T GetPropValue<T>(this object obj, string name)
        {
            var retval = GetPropertyValue(obj, name);
            if (retval == null)
                return default;
            return (T)retval;
        }
    }
}
