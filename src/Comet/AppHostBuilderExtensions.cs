using System;
using System.Collections.Generic;
using Comet.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Hosting;

namespace Comet
{
	public static class AppHostBuilderExtensions
	{
		public static IAppHostBuilder UseCometHandlers(this IAppHostBuilder builder)
		{

			//AnimationManger.SetTicker(new iOSTicker());

			//Set Default Style
			var style = new Styles.Style();
			style.Apply();

			

			builder.ConfigureMauiHandlers((_, handlersCollection) => handlersCollection.AddHandlers(new Dictionary<Type, Type>
			{
				{ typeof(AbstractLayout), typeof(LayoutHandler) },
				{ typeof(ActivityIndicator), typeof(ActivityIndicatorHandler) },
				{ typeof(Button), typeof(ButtonHandler) },
				{ typeof(CheckBox), typeof(CheckBoxHandler) },
				{ typeof(DatePicker), typeof(DatePickerHandler) },
				{ typeof(Image) , typeof(ImageHandler) },
				{ typeof(ListView),typeof(ListViewHandler) },
				//{ typeof(Picker), typeof(PickerHandler) },
				{ typeof(ProgressBar), typeof(ProgressBarHandler) },
				//{ typeof(SearchBar), typeof(SearchBarHandler) },
				{typeof(ShapeView), typeof(ShapeViewHandler)},
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
#if __IOS__
				{typeof(ScrollView), typeof(ScrollViewHandler) },
				{typeof(NavigationView), typeof (NavigationViewHandler)},
				{typeof(View), typeof(CometViewHandler)},
#endif
			}));

			return builder;
		}

	}
}