using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Modules.IdentityProvider.Application.Interfaces;
using Modules.IdentityProvider.Domain.Users;
using Modules.IdentityProvider.Domain.Users.Interfaces;
using Modules.IdentityProvider.Domain.Users.Services;
using Modules.IdentityProvider.Infrastructure.Authentication;
using Modules.IdentityProvider.Persistence;
using Modules.IdentityProvider.Persistence.Constants;
using BuildingBlocks.Persistence.Extensions;
using Modules.IdentityProvider.Endpoints.PublicAPI;
using BuildingBlocks.Persistence.Options;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using BuildingBlocks.Utilities;


namespace Modules.IdentityProvider.Infrastructure;

public static class IdentityProviderModuleInstaller
{
    public static IServiceCollection AddIdentityProviderModule(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddScoped<IJwtProvider, JwtProvider>()
            .AddScoped<ICreateUserService, CreateUserService>();

        AddPersistence(services);
        AddApplication(services, configuration);
        AddPublicAPI(services);

        return services;
    }

    private static void AddPersistence(IServiceCollection services)
    {
        services.AddDbContext<IdentityDbContext>((serviceProvider, options) =>
        {
            var connectionString = serviceProvider.GetService<IOptions<ConnectionStringOptions>>()!.Value;

            options
                .UseNpgsql(
                    connectionString,
                    dbContextOptionsBuilder => dbContextOptionsBuilder.WithMigrationHistoryTableInSchema(Schemas.Users));
        });

        services.AddIdentity<User, IdentityRole<Guid>>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 8;
        })
        .AddEntityFrameworkStores<IdentityDbContext>()
        .AddDefaultTokenProviders()
        .AddRoles<IdentityRole<Guid>>();
    }

    private static void AddApplication(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Application.AssemblyReference.Assembly);

                cfg.LicenseKey = SettingsUtils
                    .GetEnvironmentValueOrDefault("MEDIATR_KEY", configuration.GetValue<string>("MediatRKey") ?? string.Empty);
            });
    }

    private static void AddPublicAPI(IServiceCollection services)
    {
        services
            .AddScoped<IIdentityProviderEndpoints, IdentityProviderEndpoints>();
    }


}
