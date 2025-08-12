using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ParoquiaSLG.API.Authentication;

public static class JwtAuthSetup
{
    public static IServiceCollection AddJwtAuth(this IServiceCollection services)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer();

        return services;
    }
}
