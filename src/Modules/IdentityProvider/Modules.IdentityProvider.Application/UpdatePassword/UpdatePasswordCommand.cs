using BuildingBlocks.Application;

namespace Modules.IdentityProvider.Application.UpdatePassword;

public sealed record UpdatePasswordCommand(
    Guid UserId,
    string CurrentPassword,
    string NewPassword,
    string ConfirmNewPassword) : ICommand;
