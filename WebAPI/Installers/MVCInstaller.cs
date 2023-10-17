using Infrastructure;
using Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Application.Services;
using WebAPI.Middelwares;
using FluentValidation.AspNetCore;
using Swashbuckle.AspNetCore.Filters;
using Application.Validators.Attachments;
using Application.Validators.Map;
using Application.Validators.Picture;
using Application.Validators.Post;

namespace WebAPI.Installers
{
    public class MVCInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration Configuration)
        {
            services.AddApplication();
            services.AddInfrastructure();

            services.AddMemoryCache();
            services.AddControllers()
            .AddOData(options => options
            .Select()
            .Filter()
            .OrderBy());

            //Attachments
            services.AddSwaggerExamplesFromAssemblyOf<AttachmentDtoValidator>();
            services.AddSwaggerExamplesFromAssemblyOf<DownloadAttachmentDtoValidator>();
            //Map
            services.AddSwaggerExamplesFromAssemblyOf<CreateMapDtoValidator>();
            services.AddSwaggerExamplesFromAssemblyOf<UpdateMapDtoValidator>();
            //Picture
            services.AddSwaggerExamplesFromAssemblyOf<PictureDtoValidator>();
            services.AddSwaggerExamplesFromAssemblyOf<UpdatePictureDtoValidator>();
            //Post
            services.AddSwaggerExamplesFromAssemblyOf<CreatePostDtoValidator>();
            services.AddSwaggerExamplesFromAssemblyOf<UpdatePostDtoValidator>();

            services.AddApiVersioning(x =>
            {
                x.DefaultApiVersion = new ApiVersion(1, 0);
                x.AssumeDefaultVersionWhenUnspecified = true;
                x.ReportApiVersions = true;
                //x.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
            });
            services.AddTransient<UserResloverService>();
            services.AddScoped<ErrorHandlingMiddelware>();
        }
    }
}