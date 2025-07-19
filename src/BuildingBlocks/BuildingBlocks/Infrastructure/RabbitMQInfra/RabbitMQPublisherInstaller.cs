using BuildingBlocks.Application.EventBus;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Infrastructure.RabbitMQInfra;

public static class RabbitMQPublisherInstaller
{
    public static IServiceCollection AddRabbitMQ(this IServiceCollection services)
    {
        services.AddSingleton<IEventBus, RabbitMQPublisher>();

        return services;
    }
}
