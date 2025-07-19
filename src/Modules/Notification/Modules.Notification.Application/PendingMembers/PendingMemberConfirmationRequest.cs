namespace Modules.Notification.Application.PendingMembers;

public sealed record PendingMemberConfirmationRequest(
    string Email,
    string FullName,
    string Token
);
