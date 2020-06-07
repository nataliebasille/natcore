using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
namespace Natcore.AspNet.Views
{
    public static class ViewRendererExtensions
    {
        public static IServiceCollection AddViewRenderer(this IServiceCollection services)
        {
            services
                .AddScoped<IViewRenderer, RazorViewRenderer>()
                .AddMvc()
                .AddRazorOptions(o =>
                {
                    o.ViewLocationFormats.Add("/Emails/{0}" + RazorViewEngine.ViewExtension);
                });

            return services;
        }
    }
}
