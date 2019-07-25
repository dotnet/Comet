using HotUI.Blazor.Middleware.Images;
using HotUI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace HotUI.Blazor
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

            app.Map(BlazorBitmap.Prefix, app2 =>
            {
                app2.UseMiddleware<BitmapMiddleware>();
            });
        }
    }
}
