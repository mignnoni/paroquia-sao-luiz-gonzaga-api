namespace Modules.IdentityProvider.Endpoints.PublicAPI.Responses;

public sealed record UserCreated(Guid Id, string FullName, string Email);
