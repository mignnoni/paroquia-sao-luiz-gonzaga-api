using System;
using BuildingBlocks.Application.EventBus;
using Microsoft.Extensions.DependencyInjection;
using Modules.Notification.Application.ParishManagement;
using Modules.ParishManagement.IntegrationEvents.PendingMembers;

namespace Modules.Notification.Infrastructure;

public static class IntegrationEventHandlersInstaller
{
    public static IServiceCollection AddIntegrationEventHandlers(this IServiceCollection services)
    {
        services.AddScoped<IIntegrationEventHandler<PendingMemberCreatedIntegrationEvent>, PendingMemberCreatedIntegrationEventHandler>();
        return services;
    }
}
