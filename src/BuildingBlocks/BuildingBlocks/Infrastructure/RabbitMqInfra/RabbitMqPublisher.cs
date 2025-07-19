using System.Text;
using System.Text.Json;
using BuildingBlocks.Application.EventBus;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace BuildingBlocks.Infrastructure.RabbitMQInfra;

public class RabbitMQPublisher(ILogger<RabbitMQPublisher> _logger) : IEventBus, IAsyncDisposable
{
    private IConnection? _connection;
    private readonly ThreadLocal<IChannel> _threadLocalChannel = new(() => null!);

    public async Task PublishAsync<TIntegrationEvent>(TIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
        where TIntegrationEvent : IIntegrationEvent
    {
        await EnsureConnectedAsync(cancellationToken);

        var eventName = integrationEvent.GetType().Name;

        var channel = await GetOrCreateChannelAsync(cancellationToken);

        string message = JsonSerializer.Serialize(integrationEvent);
        var body = Encoding.UTF8.GetBytes(message);

        await channel.BasicPublishAsync(exchange: "event-bus", routingKey: eventName, body: body, cancellationToken: cancellationToken);

        _logger.LogInformation("Evento {EventName} publicado com sucesso", eventName);
    }

    public async Task EnsureConnectedAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_connection is null || !_connection.IsOpen)
            {
                if (_connection is not null)
                    await _connection.DisposeAsync();

                var factory = new ConnectionFactory
                {
                    HostName = "localhost",
                };

                _connection = await factory.CreateConnectionAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao estabelecer conexão com o RabbitMQ");
            throw;
        }
    }

    private async Task<IChannel> GetOrCreateChannelAsync(CancellationToken cancellationToken)
    {
        var channel = _threadLocalChannel.Value;

        if (channel is null || channel.IsClosed)
        {
            if (channel is not null)
                await channel.DisposeAsync();

            channel = await _connection!.CreateChannelAsync(cancellationToken: cancellationToken);
            await channel.ExchangeDeclareAsync(exchange: "event-bus", type: ExchangeType.Direct, cancellationToken: cancellationToken);
            _threadLocalChannel.Value = channel;
        }

        return channel;
    }

    public async ValueTask DisposeAsync()
    {
        foreach (var channel in _threadLocalChannel.Values)
        {
            if (channel is not null)
            {
                if (channel.IsOpen)
                    await channel.CloseAsync();
                await channel.DisposeAsync();
            }
        }

        if (_connection is not null)
        {
            if (_connection.IsOpen)
                await _connection.CloseAsync();
            await _connection.DisposeAsync();
        }

        GC.SuppressFinalize(this);
    }
}
