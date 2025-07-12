namespace Modules.IdentityProvider.Endpoints.PublicAPI.Requests;

public sealed record CreateUserRequest(string FullName, string Email, string Password, string Role);
