using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using WebAPI.Installers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.InstallServicesInAssembly(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddControllers();

builder.Services.AddDbContext<CSGOContext>(
    option => option.UseSqlServer(builder.Configuration.GetConnectionString("CSGODb"))
    );

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();