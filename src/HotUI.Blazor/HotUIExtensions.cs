using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace HotUI.Blazor
{
    public static class HotUIExtensions
    {
        public static void AddHotUI(this IServiceCollection services)
        {
            services.AddScoped<CanvasWriter>();
            services.AddImages();
        }

        public static void UseHotUI(this IApplicationBuilder app)
        {
            UI.Init();

            app.UseImages();
            app.Map("/_hotui/hotui.js", app2 =>
            {
                app2.Run(async ctx =>
                {
                    using (var stream = typeof(HotUIExtensions).Assembly.GetManifestResourceStream(typeof(HotUIExtensions), "Scripts.hotui.js"))
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
