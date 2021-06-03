using System;
using System.Collections.Generic;

namespace Comet.Skia.Internal
{
	public class Registration
	{
		public static Dictionary<Type, Type> DefaultRegistrarTypes = new Dictionary<Type, Type>();
		public static Dictionary<Type, Type> ReplacementRegistrarTypes = new Dictionary<Type, Type>();
		public static Dictionary<Type, Type> SkiaTypes = new Dictionary<Type, Type>();
		static Registration()
		{
			new SkiaStyle().Apply();
			Register<SKButton, Button, ButtonHandler>();
			Register<SKText, Text, TextHandler>();
			Register<SKTextField, TextField, TextFieldHandler>();
			Register<SKSlider, Slider, SliderHandler>();
			Register<SKProgressBar, ProgressBar, ProgressBarHandler>();
			Register<SKToggle, Toggle, ToggleHandler>();
		}

		static void Register<SKiaView, ReplacementView, Handler>()
		{
			DefaultRegistrarTypes[typeof(SKiaView)] = typeof(Handler);
			ReplacementRegistrarTypes[typeof(ReplacementView)] = typeof(Handler);
		}
		public static Type GenericType;
		public static void RegisterDefaultViews(Type genericType)
		{
			GenericType = genericType;
			foreach (var pair in DefaultRegistrarTypes)
				Registrar.Handlers.Register(pair.Key, genericType.MakeGenericType(pair.Value));
		}

		public static void Register<SKiaView, Handler>()
		{
			if (GenericType == null)
				throw new Exception("Please call Comet.Skia.UI.Init() first!");
			Registrar.Handlers.Register(typeof(SKiaView), GenericType.MakeGenericType(typeof(Handler)));
		}

		public static void RegisterReplacementViews(Type genericType)
		{
			foreach (var pair in ReplacementRegistrarTypes)
				Registrar.Handlers.Register(pair.Key, genericType.MakeGenericType(pair.Value));
		}
	}
}
