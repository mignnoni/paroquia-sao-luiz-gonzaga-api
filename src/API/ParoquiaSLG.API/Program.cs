using Ardalis.Result.AspNetCore;
using BuildingBlocks.Infrastructure.RabbitMQInfra;
using BuildingBlocks.Persistence.Options;
using BuildingBlocks.Utilities;
using Modules.IdentityProvider.Infrastructure;
using Modules.Notification.Infrastructure;
using Modules.ParishManagement.Infrastructure;
using ParoquiaSLG.API.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

string? allowedOrigins = SettingsUtils
    .GetEnvironmentValueOrDefault("ALLOWED_HOSTS", builder.Configuration.GetSection("Cors:AllowedOrigins").Value ?? "");

builder.Services
    .AddCors(options =>
    {
        options.AddDefaultPolicy(
            policy =>
            {
                policy
                .WithOrigins(allowedOrigins?.Split(',') ?? [])
                .AllowAnyHeader()
                .AllowAnyMethod();
            });
    });

builder.Services
    .AddMemoryCache()
    .ConfigureOptions<ConnectionStringSetup>()
    .AddRabbitMQ()
    .AddIdentityProviderModule(builder.Configuration)
    .AddParishManagementModule(builder.Configuration)
    .AddNotificationModule()
    .AddAuth();

builder.Services.AddControllers(mvcOptions => mvcOptions
    .AddResultConvention(resultStatusMap => resultStatusMap
        .AddDefaultMap()
     ));

var app = builder.Build();

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
