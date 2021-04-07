using System;
using System.Collections.Generic;
using System.Threading;
using Comet.Tests.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.HotReload;

namespace Comet.Tests
{
	public static class UI
	{
		static bool hasInit;
		public static IMauiHandlersServiceProvider Handlers { get; set; }
		public static void Init(bool force = false)
		{
			if (hasInit && !force)
				return;
			hasInit = true;
			var handlers = new Dictionary<Type, Type> {
					{ typeof(Button), typeof(GenericViewHandler)},
					{ typeof(ContentView), typeof(GenericViewHandler)},
					{ typeof(Image), typeof(GenericViewHandler)},
					{ typeof(HStack), typeof(GenericViewHandler)},
					{ typeof(ListView), typeof(GenericViewHandler)},
					{ typeof(Text), typeof(TextHandler)},
					{ typeof(TextField), typeof(TextFieldHandler)},
					{ typeof(ProgressBar), typeof(ProgressBarHandler)},
					{ typeof(SecureField), typeof(SecureFieldHandler)},
					{ typeof(ScrollView), typeof(GenericViewHandler)},
					{ typeof(Slider), typeof(SliderHandler)},
					{ typeof(Toggle), typeof(GenericViewHandler)},
					{ typeof(View), typeof(GenericViewHandler)},
					{ typeof(VStack), typeof(GenericViewHandler)},
					{ typeof(ZStack), typeof(GenericViewHandler)},
				};
			var handlerCollection = new MauiServiceCollection();
			foreach (var handler in handlers)
			{
				handlerCollection.AddTransient(handler.Key, handler.Value);
			}
			Handlers = new MauiHandlersServiceProvider(handlerCollection);

			MauiHotReloadHelper.IsEnabled = true;

			//Set Default Style
			var style = new Styles.Style();
			style.Apply();
		}
	}
}
