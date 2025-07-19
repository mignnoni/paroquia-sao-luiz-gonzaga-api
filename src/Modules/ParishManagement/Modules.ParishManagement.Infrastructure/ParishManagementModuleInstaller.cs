using Modules.ParishManagement.Infrastructure.ServiceInstallers;
using Microsoft.Extensions.DependencyInjection;

namespace Modules.ParishManagement.Infrastructure;

public static class ParishManagementModuleInstaller
{
    public static IServiceCollection AddParishManagementModule(this IServiceCollection services)
    {
        services.AddPersistence();
        services.AddApplication();
        return services;
    }
}
