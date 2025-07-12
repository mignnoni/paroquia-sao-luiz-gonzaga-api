using BuildingBlocks.Application;

namespace Modules.IdentityProvider.Application.DeleteUser;

public sealed record DeleteUserCommand(string Id) : ICommand;
