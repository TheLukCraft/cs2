namespace WebAPI.Installers
{
    public interface IInstaller
    {
        void InstallServices(IServiceCollection Services, IConfiguration Configuration);
    }
}