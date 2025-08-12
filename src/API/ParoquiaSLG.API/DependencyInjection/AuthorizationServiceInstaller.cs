using Microsoft.AspNetCore.Authorization;
using ParoquiaSLG.API.Authentication;
using ParoquiaSLG.API.Authorization;
using ParoquiaSLG.API.Authorization.Services;

namespace ParoquiaSLG.API.DependencyInjection;

public static class AuthorizationServiceInstaller
{
    public static IServiceCollection AddAuth(this IServiceCollection services)
    {
        services
            .AddAuthorization()
            .ConfigureOptions<JwtOptionsSetup>()
            .ConfigureOptions<JwtBearerOptionsSetup>()
            .AddJwtAuth()
            .AddScoped<IPermissionService, PermissionService>()
            .AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>()
            .AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        return services;
    }
}
