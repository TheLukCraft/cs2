using Infrastructure.Data;

namespace WebAPI.Installers
{
    public class HealthChecksInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddDbContextCheck<CSGOContext>("Database");

            //services.AddHealthChecksUI()
            //    .AddInMemoryStorage();
        }
    }
}