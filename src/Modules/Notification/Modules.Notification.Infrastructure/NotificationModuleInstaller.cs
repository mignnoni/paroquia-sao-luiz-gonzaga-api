using Microsoft.Extensions.DependencyInjection;
using Modules.Notification.Application.Abstractions;
using Modules.Notification.Infrastructure.Consumers;
using Modules.Notification.Infrastructure.Emails;

namespace Modules.Notification.Infrastructure;

public static class NotificationModuleInstaller
{
    public static IServiceCollection AddNotificationModule(this IServiceCollection services)
    {
        services.ConfigureOptions<EmailSenderOptionsSetup>();
        services.AddTransient<IEmailSender, EmailSender>();
        services.AddIntegrationEventHandlers();
        services.AddConsumers();

        return services;
    }
}
