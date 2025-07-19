namespace Modules.Notification.Application.ParishManagement.Requests;

public sealed record SendTokenToResetPasswordRequest(
    string FullName,
    string Email,
    string Token
);
