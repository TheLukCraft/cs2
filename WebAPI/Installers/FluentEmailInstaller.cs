using Application.Interfaces;
using Application.Services.Emails;

namespace WebAPI.Installers
{
    public class FluentEmailInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration Configuration)
        {
            services
                .AddFluentEmail(Configuration["FluentEmail:FromEmail"], Configuration["FluentEmail:FromName"])
                .AddRazorRenderer()
                .AddSmtpSender(Configuration["FluentEmail:SmtpSender:Host"],
                int.Parse(Configuration["FluentEmail:SmtpSender:Port"]),
                Configuration["FluentEmail:SmtpSender:UserName"],
                Configuration["FluentEmail:SmtpSender:Password"]);

            services.AddScoped<IEmailSenderService, EmailSenderService>();
        }
    }
}