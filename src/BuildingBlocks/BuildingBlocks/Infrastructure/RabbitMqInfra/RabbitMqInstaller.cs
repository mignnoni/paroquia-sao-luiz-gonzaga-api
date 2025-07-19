using BuildingBlocks.Application.EventBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BuildingBlocks.Infrastructure.RabbitMqInfra;

public static class RabbitMqInstaller
{
    public static IServiceCollection AddRabbitMq(this IServiceCollection services)
    {
        services.AddSingleton<IEventBus, RabbitMqPublisher>();

        return services;
    }
}
