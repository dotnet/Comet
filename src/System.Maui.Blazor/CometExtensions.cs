using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace System.Maui.Blazor
{
	public static class System.MauiExtensions
	{
		public static void AddSystem.Maui(this IServiceCollection services)
		{
			services.AddScoped<CanvasWriter>();
			services.AddImages();
		}

		public static void UseSystem.Maui(this IApplicationBuilder app)
		{
			UI.Init();

			app.UseImages();
			app.Map("/_System.Maui/System.Maui.js", app2 => {
				app2.Run(async ctx => {
					using (var stream = typeof(System.MauiExtensions).Assembly.GetManifestResourceStream(typeof(System.MauiExtensions), "Scripts.System.Maui.js"))
					{
						ctx.Response.StatusCode = 200;
						ctx.Response.ContentType = "application/javascript";

						await stream.CopyToAsync(ctx.Response.Body);
					}
				});
			});
		}
	}
}
