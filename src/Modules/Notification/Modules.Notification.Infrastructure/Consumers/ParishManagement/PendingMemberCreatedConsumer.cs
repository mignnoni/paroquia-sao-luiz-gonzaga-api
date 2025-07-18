using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using Modules.ParishManagement.IntegrationEvents.PendingMembers;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.DependencyInjection;
using BuildingBlocks.Application.EventBus;
using Microsoft.Extensions.Logging;

namespace Modules.Notification.Infrastructure.ParishManagement;

public class PendingMemberCreatedConsumer(
    IServiceProvider _serviceProvider,
    ILogger<PendingMemberCreatedConsumer> _logger) : BackgroundService
{
    private IConnection? _connection;
    private IChannel? _channel;
    private const string ExchangeName = "event-bus";
    private const string QueueName = "pending-member-created-queue";
    private const string RoutingKey = "PendingMemberCreatedIntegrationEvent";

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("[PendingMemberCreatedConsumer] Tentando conectar ao RabbitMQ...");
                await EnsureConnectedWithRetryAsync(stoppingToken);
                _logger.LogInformation("[PendingMemberCreatedConsumer] Conectado ao RabbitMQ com sucesso.");

                var consumer = new AsyncEventingBasicConsumer(_channel);

                consumer.ReceivedAsync += async (_, ea) =>
                {
                    try
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        _logger.LogInformation("[PendingMemberCreatedConsumer] Mensagem recebida: {Message}", message);

                        var integrationEvent = JsonSerializer.Deserialize<PendingMemberCreatedIntegrationEvent>(message);
                        var handler = _serviceProvider.GetRequiredService<IIntegrationEventHandler<PendingMemberCreatedIntegrationEvent>>();

                        if (integrationEvent is not null)
                        {
                            await handler.Handle(integrationEvent, stoppingToken);
                            _logger.LogInformation("[PendingMemberCreatedConsumer] Evento processado com sucesso.");
                        }
                        else
                        {
                            _logger.LogWarning("[PendingMemberCreatedConsumer] Evento de integração nulo após deserialização.");
                        }

                        await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false, cancellationToken: stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "[PendingMemberCreatedConsumer] Erro ao processar mensagem. Nack enviado.");
                        try
                        {
                            await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true, cancellationToken: stoppingToken);
                        }
                        catch (Exception nackEx)
                        {
                            _logger.LogError(nackEx, "[PendingMemberCreatedConsumer] Erro ao enviar Nack.");
                        }
                    }
                };

                await _channel.BasicConsumeAsync(queue: QueueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);

                while (_connection?.IsOpen == true && _channel?.IsOpen == true && !stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(1000, stoppingToken);
                }
                _logger.LogWarning("[PendingMemberCreatedConsumer] Conexão ou canal fechados. Tentando reconectar...");
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("[PendingMemberCreatedConsumer] Cancelamento solicitado. Encerrando consumidor.");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[PendingMemberCreatedConsumer] Erro inesperado. Tentando reconectar em 5 segundos...");
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
                _logger.LogInformation("[PendingMemberCreatedConsumer] Conexão estabelecida com RabbitMQ.");
                retryDelay = initialRetryDelay;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[PendingMemberCreatedConsumer] Falha ao conectar ao RabbitMQ. Tentando novamente em {RetryDelay} segundos...", retryDelay);
                await Task.Delay(TimeSpan.FromSeconds(retryDelay), cancellationToken);
                retryDelay = Math.Min(retryDelay * 2, maxRetryDelay);
            }
        }

        if (_connection == null || !_connection.IsOpen)
        {
            throw new Exception("[PendingMemberCreatedConsumer] Não foi possível conectar ao RabbitMQ.");
        }

        if ((_channel == null || !_channel.IsOpen) && !cancellationToken.IsCancellationRequested)
        {
            _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);
            await _channel.ExchangeDeclareAsync(ExchangeName, ExchangeType.Direct, cancellationToken: cancellationToken);
            await _channel.QueueDeclareAsync(QueueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: cancellationToken);
            await _channel.QueueBindAsync(QueueName, ExchangeName, RoutingKey, cancellationToken: cancellationToken);
            _logger.LogInformation("[PendingMemberCreatedConsumer] Canal criado e fila vinculada.");
        }
    }

    private async Task CloseConnectionAsync()
    {
        try
        {
            if (_channel?.IsOpen == true)
            {
                await _channel.CloseAsync();
                _logger.LogInformation("[PendingMemberCreatedConsumer] Canal fechado.");
            }
            if (_connection?.IsOpen == true)
            {
                await _connection.CloseAsync();
                _logger.LogInformation("[PendingMemberCreatedConsumer] Conexão fechada.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "[PendingMemberCreatedConsumer] Erro ao fechar canal/conexão.");
        }
        finally
        {
            _channel?.Dispose();
            _connection?.Dispose();
            _channel = null;
            _connection = null;
        }
    }
}
