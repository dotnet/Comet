using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace Comet
{

	public class CometApp : View, IApplication, IStartup, IPage
	{
		public CometApp()
		{
			CurrentApp = this;
		}
		public static CometApp CurrentApp { get; protected set; }
		public static CometWindow CurrentWindow { get; protected set; }
		public static IMauiContext MauiContext => CurrentWindow?.MauiContext;

		public static float DisplayScale => CurrentWindow?.DisplayScale ?? 1;

		IView IPage.Content { get => this.ReplacedView; }

		public virtual void Configure(IAppHostBuilder appBuilder)
		{
			appBuilder.ConfigureServices((context, collection) => {
				collection.AddSingleton<IApplication, CometApp>((s) => {
					return CurrentApp;
				});

			});
			appBuilder.UseCometHandlers();
		}


		IWindow IApplication.CreateWindow(IActivationState activationState)
			=> CurrentWindow = new CometWindow
			{
				Page = this,
			};


	}
}
