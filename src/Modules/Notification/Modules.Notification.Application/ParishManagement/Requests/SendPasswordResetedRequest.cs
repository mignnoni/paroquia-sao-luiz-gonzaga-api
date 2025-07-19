namespace Modules.Notification.Application.ParishManagement.Requests;

public sealed record SendPasswordResetedRequest(
    string FullName,
    string Email
);
