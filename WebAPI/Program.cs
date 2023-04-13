using Application.Interfaces;
using Application.Mappings;
using Application.Services;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IPostService, PostService>();

builder.Services.AddSingleton(AutoMapperConfig.Initialize());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
});

builder.Services.AddApiVersioning(x =>
{
    x.DefaultApiVersion = new ApiVersion(1, 0);
    x.AssumeDefaultVersionWhenUnspecified = true;
    x.ReportApiVersions = true;
    x.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
});
builder.Services.AddDbContext<CSGOContext>(
    option => option.UseSqlServer(builder.Configuration.GetConnectionString("CSGODb"))
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();