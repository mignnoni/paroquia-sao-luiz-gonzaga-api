namespace ParoquiaSLG.API.Modules.ParishManagement.Members.Contracts;

public sealed record ResetPasswordRequest(
    string Token,
    string NewPassword,
    string ConfirmNewPassword);
