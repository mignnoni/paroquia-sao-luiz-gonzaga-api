using System;
using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.ParishManagement.Application.Abstractions;
using Modules.ParishManagement.Infrastructure.Services.AWS;

namespace Modules.ParishManagement.Infrastructure.ServiceInstallers;

public static class InfrastructureInstaller
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAWSService<IAmazonS3>(configuration.GetAWSOptions());

        services.ConfigureOptions<S3ServiceOptionsSetup>();

        services.AddSingleton<IS3Service, S3Service>();

        return services;
    }
}
