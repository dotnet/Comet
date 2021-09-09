using System;
using System.Linq;
using Comet.Graphics;
using Comet.Samples.Models;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Hosting;

namespace Comet.Samples
{
	public class MyApp : CometApp
	{
		[Body]
		View view() => new MainPage();

		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder.UseCometApp<MyApp>()
				.ConfigureFonts(fonts => {
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				});
#if DEBUG
			builder.EnableHotReload();
#endif

			return builder.Build();
		}
	}
}
