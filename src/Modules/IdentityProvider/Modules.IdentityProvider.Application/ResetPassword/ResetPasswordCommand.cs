using BuildingBlocks.Application;

namespace Modules.IdentityProvider.Application.ResetPassword;

public sealed record ResetPasswordCommand(
    string Email,
    string Token, 
    string NewPassword, 
    string ConfirmNewPassword) : ICommand;
