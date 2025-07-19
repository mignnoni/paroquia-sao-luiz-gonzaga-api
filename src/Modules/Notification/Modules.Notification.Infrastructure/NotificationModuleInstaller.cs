using Microsoft.Extensions.DependencyInjection;
using Modules.Notification.Infrastructure.Consumers;

namespace Modules.Notification.Infrastructure;

public static class NotificationModuleInstaller
{
    public static IServiceCollection AddNotificationModule(this IServiceCollection services)
    {
        services.AddIntegrationEventHandlers();
        services.AddConsumers();

        return services;
    }
}
