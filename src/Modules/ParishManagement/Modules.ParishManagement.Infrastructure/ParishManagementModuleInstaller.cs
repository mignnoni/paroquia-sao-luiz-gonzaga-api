using Modules.ParishManagement.Infrastructure.ServiceInstallers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Modules.ParishManagement.Infrastructure;

public static class ParishManagementModuleInstaller
{
    public static IServiceCollection AddParishManagementModule(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddPersistence()
            .AddApplication()
            .AddInfrastructure(configuration);

        return services;
    }
}
