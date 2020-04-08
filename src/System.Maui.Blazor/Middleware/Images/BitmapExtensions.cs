using System.Maui.Blazor.Middleware.Images;
using System.Maui.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace System.Maui.Blazor
{
	internal static class BitmapExtensions
	{
		public static void AddImages(this IServiceCollection services)
		{
			services.AddSingleton<BitmapRepository>();
			services.AddSingleton<IBitmapService, BlazorBitmapService>();
			services.AddHttpClient<BitmapMiddleware>();
		}

		public static void UseImages(this IApplicationBuilder app)
		{
			Device.BitmapService = app.ApplicationServices.GetRequiredService<IBitmapService>();

			app.Map(BlazorBitmap.Prefix, app2 => {
				app2.UseMiddleware<BitmapMiddleware>();
			});
		}
	}
}
