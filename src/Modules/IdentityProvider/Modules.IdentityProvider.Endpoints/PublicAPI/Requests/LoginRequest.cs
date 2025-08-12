namespace Modules.IdentityProvider.Endpoints.PublicAPI.Requests;

public sealed record LoginRequest(string Email, string Password);