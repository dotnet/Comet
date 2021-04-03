using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace Comet
{

	public class CometApp : View, IApplication, IStartup, IPage
	{

		public IMauiContext MauiContext { get; set; }
		IView IPage.View { get => this.ReplacedView; set => throw new NotImplementedException(); }

		public virtual void Configure(IAppHostBuilder appBuilder)
		{
			appBuilder.UseCometHandlers();
		}

		IWindow IApplication.CreateWindow(IActivationState activationState) => new CometWindow
		{
			MauiContext = MauiContext,
			Page = this,
		};
	}
}
