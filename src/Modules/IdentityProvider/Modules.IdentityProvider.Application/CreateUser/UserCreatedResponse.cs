namespace Modules.IdentityProvider.Application.CreateUser;

public sealed record UserCreatedResponse(Guid Id, string FullName, string Email);
