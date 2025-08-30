using BuildingBlocks.Persistence.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Modules.ParishManagement.Persistence;
using Microsoft.EntityFrameworkCore;
using Modules.ParishManagement.Persistence.Constants;
using BuildingBlocks.Persistence.Extensions;
using Modules.ParishManagement.Domain.Abstractions;
using Modules.ParishManagement.Persistence.Repositories;

namespace Modules.ParishManagement.Infrastructure.ServiceInstallers;

public static class PersistenceInstaller
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddDatabase();
        services.AddRepositories();

        return services;
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IPendingMemberRepository, PendingMemberRepository>();
        services.AddScoped<IMemberRepository, MemberRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IMassLocationRepository, MassLocationRepository>();
        services.AddScoped<IOtherScheduleRepository, OtherScheduleRepository>();
        services.AddScoped<INewsRepository, NewsRepository>();
    }

    private static void AddDatabase(this IServiceCollection services)
    {
        services
    .AddDbContext<ParishManagementDbContext>((serviceProvider, options) =>
    {
        ConnectionStringOptions connectionString = serviceProvider.GetService<IOptions<ConnectionStringOptions>>()!.Value;

        options
            .UseNpgsql(
                connectionString,
                dbContextOptionsBuilder => dbContextOptionsBuilder.WithMigrationHistoryTableInSchema(Schemas.ParishManagement))
            .UseSnakeCaseNamingConvention();
    });
    }
}
