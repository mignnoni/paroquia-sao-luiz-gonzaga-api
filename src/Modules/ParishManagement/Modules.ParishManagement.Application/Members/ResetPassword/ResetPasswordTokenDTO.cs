namespace Modules.ParishManagement.Application.Members.ResetPassword;

public sealed record ResetPasswordTokenDTO(
    string Token,
    string Email);
