using BuildingBlocks.Application.EventBus;
using Microsoft.Extensions.Logging;
using Modules.Notification.Application.Abstractions;
using Modules.Notification.Application.PendingMembers;
using Modules.ParishManagement.IntegrationEvents.PendingMembers;

namespace Modules.Notification.Application.ParishManagement;

public class PendingMemberCreatedIntegrationEventHandler(
    IEmailSender _emailSender,
    ILogger<PendingMemberCreatedIntegrationEventHandler> _logger
) : IIntegrationEventHandler<PendingMemberCreatedIntegrationEvent>
{
    public async Task Handle(PendingMemberCreatedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        try
        {
            await _emailSender
                .PendingMemberConfirmation(new PendingMemberConfirmationRequest(
                    integrationEvent.Email,
                    integrationEvent.FullName,
                    integrationEvent.Token), cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao enviar email de confirmação de cadastro de membro para {Email}", integrationEvent.Email);
        }
    }
}
