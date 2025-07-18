
using Microsoft.Extensions.Hosting;

namespace BuildingBlocks.Infrastructure.RabbitMqInfra;

public class RabbitMqHostedService : IHostedService, IAsyncDisposable
{
    public RabbitMqPublisher Publisher { get; private set; } = null!;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Publisher = await RabbitMqPublisher.CreateConnectionAsync(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (Publisher != null)
        {
            await Publisher.DisposeAsync();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (Publisher != null)
        {
            await Publisher.DisposeAsync();
        }

        GC.SuppressFinalize(this);
    }
}
