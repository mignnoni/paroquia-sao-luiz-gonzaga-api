using BuildingBlocks.Application.EventBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BuildingBlocks.Infrastructure.RabbitMqInfra;

public static class RabbitMqInstaller
{
    public static void AddRabbitMq(this IServiceCollection services)
    {
        services.AddSingleton<IHostedService, RabbitMqHostedService>();
        services.AddSingleton<IEventBus>(sp =>
        {
            var hostedService = sp.GetRequiredService<RabbitMqHostedService>();
            return hostedService.Publisher;
        });
    }
}
