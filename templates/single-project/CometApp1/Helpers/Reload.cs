#nullable enable
//-:cnd
#if DEBUG
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Maui.HotReload;
using Esp.Resources;
using System.Reflection;
using Microsoft.Maui.Hosting;
using Microsoft.Maui;

namespace Comet
{
	public static partial class Reload
	{
		public static MauiAppBuilder EnableHotReload(this MauiAppBuilder builder, string? ideIp = null, int idePort = Constants.DEFAULT_PORT)
		{
			builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IMauiInitializeService, HotReloadBuilder>(_ => new HotReloadBuilder
			{
				IdeIp = ideIp,
				IdePort = idePort,
			}));
			return builder;
		}

		class HotReloadBuilder : IMauiInitializeService
		{
			public string? IdeIp { get; set; }

			public int IdePort { get; set; } = 9988;

			int _attempts = 3;

			public async void Initialize(IServiceProvider services)
			{
				var handlers = services.GetRequiredService<IMauiHandlersFactory>();

				Reloadify.Reload.Instance.ReplaceType = (d) =>
				{
					MauiHotReloadHelper.RegisterReplacedView(d.ClassName, d.Type);
				};

				Reloadify.Reload.Instance.FinishedReload = () =>
				{
					MauiHotReloadHelper.TriggerReload();
				};

				await Task.Run(async () =>
				{
					try
					{
						var success = await Reloadify.Reload.Init(IdeIp, IdePort);

						while(!success && _attempts > 0)
						{
							await Task.Delay(TimeSpan.FromSeconds(5));
							Console.WriteLine("Trying to connect HotReload server....");
							success = await Reloadify.Reload.Init(IdeIp, IdePort);
							_attempts--;
						}

						Console.WriteLine($"HotReload Initialize: {success}");
					}
					catch (Exception ex)
					{
						Console.WriteLine(ex);
					}
				});
			}
		}
	}
}
#endif
//+:cnd