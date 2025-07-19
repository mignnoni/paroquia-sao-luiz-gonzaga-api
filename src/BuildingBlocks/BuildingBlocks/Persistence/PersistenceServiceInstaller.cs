using BuildingBlocks.Persistence.Options;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Persistence;

public static class PersistenceServiceInstaller
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services
            .AddMemoryCache()
            .ConfigureOptions<ConnectionStringSetup>();

        return services;
    }
}
