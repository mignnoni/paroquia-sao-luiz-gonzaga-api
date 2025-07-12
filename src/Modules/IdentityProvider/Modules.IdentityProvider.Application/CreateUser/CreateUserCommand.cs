using BuildingBlocks.Application;

namespace Modules.IdentityProvider.Application.CreateUser;

public sealed record CreateUserCommand(string FullName, string Email, string Password, string Role) : ICommand<UserCreatedResponse>;
