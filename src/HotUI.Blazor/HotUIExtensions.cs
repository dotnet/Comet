using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace HotUI.Blazor
{
    public static class HotUIExtensions
    {
        public static void AddHotUI(this IServiceCollection services)
        {
            services.AddImages();
        }

        public static void UseHotUI(this IApplicationBuilder app)
        {
            UI.Init();
            app.UseImages();
        }
    }
}
