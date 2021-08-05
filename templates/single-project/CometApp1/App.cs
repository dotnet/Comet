using System;
using System.Linq;
using Comet.Graphics;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Hosting;
using System.Collections.Generic;
using Comet;
using System.IO;

namespace CometApp1
{
	public class App : CometApp
	{
		[Body]
		View view() => new MainPage();

		public override void Configure(IAppHostBuilder appBuilder)
		{
			base.Configure(appBuilder);			
//-:cnd
#if DEBUG
			appBuilder.EnableHotReload();
#endif
//+:cnd
		}
	}
}
