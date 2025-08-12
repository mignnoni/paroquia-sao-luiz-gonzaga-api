using BuildingBlocks.Application;

namespace Modules.IdentityProvider.Application.Login
{
    public sealed record LoginCommand(string Email, string Password) : ICommand<LoginResponse>;
}
