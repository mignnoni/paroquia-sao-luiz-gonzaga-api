using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
using System;


namespace Modules.IdentityProvider.Infrastructure;

public static class UserAccessModuleInstaller
{
    public static IServiceCollection AddUserModule(this IServiceCollection services)
    {
        services
            .AddScoped<IJwtProvider, JwtProvider>()
            .AddScoped<ICreateUserService, CreateUserService>();

        AddPersistence(services);
        AddApplication(services);
        AddPublicAPI(services);

        return services;
    }

    private static void AddPersistence(IServiceCollection services)
    {
        services.AddDbContext<UsersDbContext>((serviceProvider, options) =>
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
        .AddEntityFrameworkStores<UsersDbContext>()
        .AddDefaultTokenProviders()
        .AddRoles<IdentityRole<Guid>>();
    }

    private static void AddApplication(IServiceCollection services)
    {
        services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Application.AssemblyReference.Assembly));
    }

    private static void AddPublicAPI(IServiceCollection services)
    {
        services
            .AddScoped<IIdentityProviderEndpoints, IdentityProviderEndpoints>();
    }


}
