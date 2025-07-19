using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.DependencyInjection;
using BuildingBlocks.Application.EventBus;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Infrastructure.RabbitMQInfra;

public class RabbitMQConsumer<TIntegrationEvent>(
    IServiceScopeFactory _serviceScopeFactory,
    ILogger<RabbitMQConsumer<TIntegrationEvent>> _logger) : BackgroundService
    where TIntegrationEvent : IIntegrationEvent
{
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly string _exchangeName = "event-bus";
    private readonly string _queueName = $"{typeof(TIntegrationEvent).Name}-queue";
    private readonly string _routingKey = typeof(TIntegrationEvent).Name;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("{ConsumerName} Tentando conectar ao RabbitMQ...", _queueName);
                await EnsureConnectedWithRetryAsync(stoppingToken);
                _logger.LogInformation("{ConsumerName} Conectado ao RabbitMQ com sucesso.", _queueName);

                if (_channel == null)
                {
                    _logger.LogError("{ConsumerName} Canal não foi inicializado corretamente.", _queueName);
                    continue;
                }

                var consumer = new AsyncEventingBasicConsumer(_channel);

                consumer.ReceivedAsync += async (_, ea) =>
                {
                    try
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        _logger.LogInformation("{ConsumerName} Mensagem recebida.", _queueName);

                        var integrationEvent = JsonSerializer.Deserialize<TIntegrationEvent>(message);

                        using var scope = _serviceScopeFactory.CreateScope();
                        var handler = scope.ServiceProvider.GetRequiredService<IIntegrationEventHandler<TIntegrationEvent>>();

                        if (integrationEvent is not null)
                        {
                            await handler.Handle(integrationEvent, stoppingToken);
                            _logger.LogInformation("{ConsumerName} Evento processado com sucesso.", _queueName);
                        }
                        else
                        {
                            _logger.LogWarning("{ConsumerName} Evento de integração nulo após deserialização.", _queueName);
                        }

                        if (_channel?.IsOpen == true)
                        {
                            await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false, cancellationToken: stoppingToken);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "{ConsumerName} Erro ao processar mensagem. Nack enviado.", _queueName);
                        try
                        {
                            if (_channel?.IsOpen == true)
                            {
                                await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true, cancellationToken: stoppingToken);
                            }
                        }
                        catch (Exception nackEx)
                        {
                            _logger.LogError(nackEx, "{ConsumerName} Erro ao enviar Nack.", _queueName);
                        }
                    }
                };

                if (_channel?.IsOpen == true)
                {
                    await _channel.BasicConsumeAsync(queue: _queueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
                }

                while (_connection?.IsOpen == true && _channel?.IsOpen == true && !stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(1000, stoppingToken);
                }
                _logger.LogWarning("{ConsumerName} Conexão ou canal fechados. Tentando reconectar...", _queueName);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("{ConsumerName} Cancelamento solicitado. Encerrando consumidor.", _queueName);
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{ConsumerName} Erro inesperado. Tentando reconectar em 5 segundos...", _queueName);
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
            finally
            {
                await CloseConnectionAsync();
            }
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel?.IsOpen == true)
            await _channel.CloseAsync(cancellationToken);

        if (_connection?.IsOpen == true)
            await _connection.CloseAsync(cancellationToken);

        _channel?.Dispose();
        _connection?.Dispose();

        await base.StopAsync(cancellationToken: cancellationToken);
    }

    private async Task EnsureConnectedWithRetryAsync(CancellationToken cancellationToken)
    {
        int initialRetryDelay = 5;
        int maxRetryDelay = 60;
        int retryDelay = initialRetryDelay;
        while ((_connection == null || !_connection.IsOpen) && !cancellationToken.IsCancellationRequested)
        {
            try
            {
                var factory = new ConnectionFactory { HostName = "localhost" };
                _connection = await factory.CreateConnectionAsync(cancellationToken);
                _logger.LogInformation("{ConsumerName} Conexão estabelecida com RabbitMQ.", _queueName);
                retryDelay = initialRetryDelay;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{ConsumerName} Falha ao conectar ao RabbitMQ. Tentando novamente em {RetryDelay} segundos...", _queueName, retryDelay);
                await Task.Delay(TimeSpan.FromSeconds(retryDelay), cancellationToken);
                retryDelay = Math.Min(retryDelay * 2, maxRetryDelay);
            }
        }

        if (_connection == null || !_connection.IsOpen)
        {
            throw new Exception($"{_queueName} Não foi possível conectar ao RabbitMQ.");
        }

        if ((_channel == null || !_channel.IsOpen) && !cancellationToken.IsCancellationRequested)
        {
            _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);
            await _channel.ExchangeDeclareAsync(_exchangeName, ExchangeType.Direct, cancellationToken: cancellationToken);
            await _channel.QueueDeclareAsync(_queueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: cancellationToken);
            await _channel.QueueBindAsync(_queueName, _exchangeName, _routingKey, cancellationToken: cancellationToken);
            _logger.LogInformation("{ConsumerName} Canal criado e fila vinculada.", _queueName);
        }
    }

    private async Task CloseConnectionAsync()
    {
        try
        {
            if (_channel?.IsOpen == true)
            {
                await _channel.CloseAsync();
                _logger.LogInformation("{ConsumerName} Canal fechado.", _queueName);
            }
            if (_connection?.IsOpen == true)
            {
                await _connection.CloseAsync();
                _logger.LogInformation("{ConsumerName} Conexão fechada.", _queueName);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "{ConsumerName} Erro ao fechar canal/conexão.", _queueName);
        }
        finally
        {
            _channel?.Dispose();
            _connection?.Dispose();
            _connection = null;
            _channel = null;
        }
    }
}
