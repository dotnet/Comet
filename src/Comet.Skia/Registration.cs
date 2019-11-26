using System;
using System.Collections.Generic;

namespace Comet.Skia.Internal
{
    public class Registration
    {
        public static Dictionary<Type, Type> DefaultRegistrarTypes = new Dictionary<Type, Type>();
        public static Dictionary<Type, Type> ReplacementRegistrarTypes = new Dictionary<Type, Type>();

        static Registration()
        {
            Register<SKButton, Button, ButtonHandler>();
            Register<SKText, Text, TextHandler>();
        }

        static void Register<SKiaView, ReplacementView, Handler>()
        {
            DefaultRegistrarTypes[typeof(SKiaView)] = typeof(Handler);
            ReplacementRegistrarTypes[typeof(ReplacementView)] = typeof(Handler);
        }

        public static void RegisterDefaultViews(Type genericType)
        {
            foreach (var pair in DefaultRegistrarTypes)
                Registrar.Handlers.Register(pair.Key, genericType.MakeGenericType(pair.Value));
        }
        public static void RegisterReplacementViews(Type genericType)
        {
            foreach (var pair in ReplacementRegistrarTypes)
                Registrar.Handlers.Register(pair.Key, genericType.MakeGenericType(pair.Value));
        }
    }
}
