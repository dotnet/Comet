using System;
using System.Collections.Generic;
using Comet.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Maui;
using Microsoft.Maui.Essentials;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Hosting;

#if WINDOWS
using Microsoft.Maui.Graphics.Win2D;
#endif

namespace Comet
{
	public static class AppHostBuilderExtensions
	{
		static void AddHandlers(this IMauiHandlersCollection collection, Dictionary<Type, Type> handlers) => handlers.ForEach(x => collection.AddHandler(x.Key, x.Value));
		public static MauiAppBuilder UseCometApp<TApp>(this MauiAppBuilder builder)
			where TApp : class, IApplication
		{
			builder.Services.TryAddSingleton<IApplication, TApp>();
			builder.UseCometHandlers();
			return builder;
		}
		public static MauiAppBuilder UseCometHandlers(this MauiAppBuilder builder)
		{

			//AnimationManger.SetTicker(new iOSTicker());

			//Set Default Style
			var style = new Styles.Style();
			style.Apply();

			ViewHandler.ViewMapper.AppendToMapping(nameof(IGestureView.Gestures), CometViewHandler.AddGestures);
			ViewHandler.ViewCommandMapper.AppendToMapping(Gesture.AddGestureProperty, CometViewHandler.AddGesture);
			ViewHandler.ViewCommandMapper.AppendToMapping(Gesture.AddGestureProperty, CometViewHandler.RemoveGesture);
			builder.ConfigureMauiHandlers((handlersCollection) => handlersCollection.AddHandlers(new Dictionary<Type, Type>
			{
				{ typeof(AbstractLayout), typeof(LayoutHandler) },
				{ typeof(ActivityIndicator), typeof(ActivityIndicatorHandler) },
				{ typeof(Button), typeof(ButtonHandler) },
				{ typeof(CheckBox), typeof(CheckBoxHandler) },
				{ typeof(CometWindow), typeof(WindowHandler) },
				{ typeof(DatePicker), typeof(DatePickerHandler) },
				{ typeof(FlyoutView), typeof(FlyoutViewHandler) },
				{ typeof(GraphicsView), typeof(GraphicsViewHandler) },
				{ typeof(Image) , typeof(ImageHandler) },
				//{ typeof(Picker), typeof(PickerHandler) },
				{ typeof(ProgressBar), typeof(ProgressBarHandler) },
				{ typeof(SearchBar), typeof(SearchBarHandler) },
				{ typeof(SecureField), typeof(EntryHandler) },
				{ typeof(Slider), typeof(SliderHandler) },
				{ typeof(Stepper), typeof(StepperHandler) },
				{ typeof(Spacer), typeof(SpacerHandler) },
				{ typeof(TabView), typeof(TabViewHandler) },
				{ typeof(TextEditor), typeof(EditorHandler) },
				{ typeof(TextField), typeof(EntryHandler) },
				{ typeof(Text), typeof(LabelHandler) },
				{ typeof(TimePicker), typeof(TimePickerHandler) },
				{ typeof(Toggle), typeof(SwitchHandler) },
				{ typeof(Toolbar), typeof(ToolbarHandler) },
				{ typeof(CometApp), typeof(ApplicationHandler) },
				{ typeof(ListView),typeof(ListViewHandler) },
#if __MOBILE__
				{typeof(ScrollView), typeof(Handlers.ScrollViewHandler) },
				{typeof(ShapeView), typeof(Handlers.ShapeViewHandler)},
#else
				
				{typeof(ScrollView), typeof(Microsoft.Maui.Handlers.ScrollViewHandler) },
#endif


#if __IOS__
				{typeof(NavigationView), typeof (Handlers.NavigationViewHandler)},
				{typeof(View), typeof(CometViewHandler)},
#else
				
				{typeof(NavigationView), typeof (Microsoft.Maui.Handlers.NavigationViewHandler)},
#endif
			}));


#if WINDOWS
			var dictionaries = Microsoft.UI.Xaml.Application.Current?.Resources?.MergedDictionaries;
			if (dictionaries != null)
			{
				// WinUI
				AddLibraryResources<Microsoft.UI.Xaml.Controls.XamlControlsResources>();

				// Microsoft.Maui
				AddLibraryResources("MicrosoftMauiCoreIncluded", "ms-appx:///Microsoft.Maui/Platform/Windows/Styles/Resources.xbf");
			}

#endif
			ThreadHelper.SetFireOnMainThread(MainThread.BeginInvokeOnMainThread);

			return builder;
		}


#if WINDOWS
			static void AddLibraryResources(string key, string uri)
			{
				var resources = Microsoft.UI.Xaml.Application.Current?.Resources;
				if (resources == null)
					return;

				var dictionaries = resources.MergedDictionaries;
				if (dictionaries == null)
					return;

				if (!resources.ContainsKey(key))
				{
					dictionaries.Add(new Microsoft.UI.Xaml.ResourceDictionary
					{
						Source = new Uri(uri)
					});
				}
			}

			static void AddLibraryResources<T>()
				where T : Microsoft.UI.Xaml.ResourceDictionary, new()
			{
				var dictionaries = Microsoft.UI.Xaml.Application.Current?.Resources?.MergedDictionaries;
				if (dictionaries == null)
					return;

				var found = false;
				foreach (var dic in dictionaries)
				{
					if (dic is T)
					{
						found = true;
						break;
					}
				}

				if (!found)
				{
					var dic = new T();
					dictionaries.Add(dic);
				}
			}
#endif
	}
}