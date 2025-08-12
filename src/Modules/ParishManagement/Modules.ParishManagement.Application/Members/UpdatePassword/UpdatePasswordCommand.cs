using BuildingBlocks.Application;

namespace Modules.ParishManagement.Application.Members.UpdatePassword;

public sealed record UpdatePasswordCommand(
    Guid IdentityId,
    string CurrentPassword,
    string NewPassword,
    string ConfirmNewPassword) : ICommand;