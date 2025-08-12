namespace ParoquiaSLG.API.Modules.ParishManagement.Members.Contracts;

public sealed record UpdatePasswordRequest(
    string CurrentPassword,
    string NewPassword,
    string ConfirmNewPassword);