using BuildingBlocks.Infrastructure.RabbitMQInfra;
using Microsoft.Extensions.DependencyInjection;
using Modules.ParishManagement.IntegrationEvents.PendingMembers;

namespace Modules.Notification.Infrastructure.Consumers;


public static class NotificationConsumersInstaller
{
    public static IServiceCollection AddConsumers(this IServiceCollection services)
    {
        services.AddHostedService<RabbitMQConsumer<PendingMemberCreatedIntegrationEvent>>();
        return services;
    }
}