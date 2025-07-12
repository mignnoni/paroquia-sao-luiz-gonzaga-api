namespace Modules.IdentityProvider.Endpoints.PublicAPI.Responses;

public sealed record ForgottenPasswordResponse(string Email, string Token);
