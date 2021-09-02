using System;
using System.Collections.Generic;
using Comet.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Maui;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Hosting;

namespace Comet
{
	public static class AppHostBuilderExtensions
	{
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

			builder.ConfigureMauiHandlers((handlersCollection) => handlersCollection.AddHandlers(new Dictionary<Type, Type>
			{
				{ typeof(AbstractLayout), typeof(LayoutHandler) },
				{ typeof(ActivityIndicator), typeof(ActivityIndicatorHandler) },
				{ typeof(Button), typeof(ButtonHandler) },
				{ typeof(CheckBox), typeof(CheckBoxHandler) },
				{ typeof(CometWindow), typeof(WindowHandler) },
				{ typeof(DatePicker), typeof(DatePickerHandler) },
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
#if __ANDROID__
				
				{typeof(NavigationView), typeof (Microsoft.Maui.Handlers.NavigationViewHandler)},
#elif __MOBILE__
				{typeof(NavigationView), typeof (Handlers.NavigationViewHandler)},
#endif

#if __MOBILE__
				{ typeof(ListView),typeof(ListViewHandler) },
				{typeof(ScrollView), typeof(Handlers.ScrollViewHandler) },
				{typeof(ShapeView), typeof(Handlers.ShapeViewHandler)},

#endif


#if __IOS__
				{typeof(View), typeof(CometViewHandler)},
#endif
			}));

			return builder;
		}

	}
}