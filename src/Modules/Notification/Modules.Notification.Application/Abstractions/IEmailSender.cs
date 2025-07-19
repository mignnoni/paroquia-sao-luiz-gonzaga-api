using System;
using Modules.Notification.Application.ParishManagement.Requests;
using Modules.Notification.Application.PendingMembers;

namespace Modules.Notification.Application.Abstractions;

public interface IEmailSender
{
    Task SendTokenToResetPassword(SendTokenToResetPasswordRequest request, CancellationToken cancellationToken = default);
    Task PasswordReseted(SendPasswordResetedRequest request, CancellationToken cancellationToken = default);
    Task PendingMemberConfirmation(PendingMemberConfirmationRequest request, CancellationToken cancellationToken = default);
}
