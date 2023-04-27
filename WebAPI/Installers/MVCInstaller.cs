﻿using Infrastructure;
using Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Application.Services;
using WebAPI.Middelwares;
using FluentValidation;
using Application.Validators;
using FluentValidation.AspNetCore;

namespace WebAPI.Installers
{
    public class MVCInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration Configuration)
        {
            services.AddApplication();
            services.AddInfrastructure();
            services.AddControllers()
            .AddOData(options => options
            .Select()
            .Filter()
            .OrderBy());
            services.AddFluentValidation(options =>
            {
                options.RegisterValidatorsFromAssemblyContaining<CreatePostDtoValidator>();
            });
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