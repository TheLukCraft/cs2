using Application.Interfaces;
using Application.Mappings;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IPostService, PostService>();
            services.AddSingleton(AutoMapperConfig.Initialize());
            services.AddScoped<IPictureService, PictureService>();
            services.AddScoped<IAttachmentService, AttachmentService>();
            services.AddScoped<IMapService, MapService>();

            return services;
        }
    }
}