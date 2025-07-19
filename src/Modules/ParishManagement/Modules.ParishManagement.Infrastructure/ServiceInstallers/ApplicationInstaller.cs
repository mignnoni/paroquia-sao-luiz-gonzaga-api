using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Modules.ParishManagement.Infrastructure.ServiceInstallers;

public static class ApplicationInstaller
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Application.AssemblyReference.Assembly))
            .AddValidatorsFromAssembly(Application.AssemblyReference.Assembly);

        return services;
    }
}
