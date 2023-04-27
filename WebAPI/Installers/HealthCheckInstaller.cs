using Infrastructure.Data;

namespace WebAPI.Installers
{
    public class HealthCheckInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddSqlServer(configuration.GetConnectionString("ConnectionStrings"));

            services.AddHealthChecksUI()
                .AddInMemoryStorage();
        }
    }
}