using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Hosting;

namespace Comet
{
	public static class AppHostBuilderExtensions
	{
		public static IAppHostBuilder UseMauiHandlers(this IAppHostBuilder builder)
		{

			//AnimationManger.SetTicker(new iOSTicker());

			//Set Default Style
			var style = new Styles.Style();
			style.Apply();

			builder.RegisterHandlers(new Dictionary<Type, Type>
			{
				{ typeof(ActivityIndicator), typeof(ActivityIndicatorHandler) },
				{ typeof(Button), typeof(ButtonHandler) },
				//{ typeof(ICheck), typeof(CheckBoxHandler) },
				{ typeof(DatePicker), typeof(DatePickerHandler) },
				//{ typeof(Editor), typeof(EditorHandler) },
				{ typeof(TextField), typeof(EntryHandler) },
				{ typeof(Text), typeof(LabelHandler) },
				{ typeof(AbstractLayout), typeof(LayoutHandler) },
				//{ typeof(Picker), typeof(PickerHandler) },
				//{ typeof(Progress), typeof(ProgressBarHandler) },
				//{ typeof(SearchBar), typeof(SearchBarHandler) },
				{ typeof(Slider), typeof(SliderHandler) },
				{ typeof(Stepper), typeof(StepperHandler) },
				//{ typeof(Switch), typeof(SwitchHandler) },
				//{ typeof(TimePicker), typeof(TimePickerHandler) },
			});

			return builder;
		}

	}
}