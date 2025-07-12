using BuildingBlocks.Application;

namespace Modules.IdentityProvider.Application.UpdateUser;

public sealed record UpdateUserCommand(
    Guid UserRequestId,
    Guid UserId,
    string Name,
    string OldRole,
    string? NewRole = null) : ICommand;
