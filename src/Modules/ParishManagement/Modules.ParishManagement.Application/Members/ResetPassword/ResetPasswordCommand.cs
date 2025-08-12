using BuildingBlocks.Application;

namespace Modules.ParishManagement.Application.Members.ResetPassword;

public sealed record ResetPasswordCommand(
    string Token,
    string NewPassword,
    string ConfirmNewPassword) : ICommand;
