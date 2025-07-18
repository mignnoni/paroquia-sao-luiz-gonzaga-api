using System.Text;
using System.Text.Json;
using BuildingBlocks.Application.EventBus;
using RabbitMQ.Client;

namespace BuildingBlocks.Infrastructure.RabbitMqInfra;

public class RabbitMqPublisher(IConnection connection) : IEventBus, IAsyncDisposable
{
    private readonly IConnection _connection = connection;
    private readonly ThreadLocal<IChannel> _threadLocalChannel = new(() => null!);

    public async Task PublishAsync<TIntegrationEvent>(TIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
        where TIntegrationEvent : IIntegrationEvent
    {
        var eventName = integrationEvent.GetType().Name;
        var channel = await GetOrCreateChannelAsync(cancellationToken);

        string message = JsonSerializer.Serialize(integrationEvent);
        var body = Encoding.UTF8.GetBytes(message);

        await channel.BasicPublishAsync(exchange: "event-bus", routingKey: eventName, body: body, cancellationToken: cancellationToken);
    }

    public static async Task<RabbitMqPublisher> CreateConnectionAsync(CancellationToken cancellationToken = default)
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
        };

        var connection = await factory.CreateConnectionAsync(cancellationToken);

        return new RabbitMqPublisher(connection);
    }

    private async Task<IChannel> GetOrCreateChannelAsync(CancellationToken cancellationToken)
    {
        var channel = _threadLocalChannel.Value;
        if (channel == null || channel.IsClosed)
        {
            if (channel != null)
                await channel.DisposeAsync();

            channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);
            await channel.ExchangeDeclareAsync(exchange: "event-bus", type: ExchangeType.Direct, cancellationToken: cancellationToken);
            _threadLocalChannel.Value = channel;
        }
        return channel;
    }

    public async ValueTask DisposeAsync()
    {
        foreach (var channel in _threadLocalChannel.Values)
        {
            if (channel != null)
            {
                if (channel.IsOpen)
                    await channel.CloseAsync();
                await channel.DisposeAsync();
            }
        }

        if (_connection != null)
        {
            if (_connection.IsOpen)
                await _connection.CloseAsync();
            await _connection.DisposeAsync();
        }

        GC.SuppressFinalize(this);
    }
}
