using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Comet.Blazor
{
	public static class CometExtensions
	{
		public static void AddComet(this IServiceCollection services)
		{
			services.AddScoped<CanvasWriter>();
			services.AddImages();
		}

		public static void UseComet(this IApplicationBuilder app)
		{
			UI.Init();

			app.UseImages();
			app.Map("/_comet/comet.js", app2 => {
				app2.Run(async ctx => {
					using (var stream = typeof(CometExtensions).Assembly.GetManifestResourceStream(typeof(CometExtensions), "Scripts.comet.js"))
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
