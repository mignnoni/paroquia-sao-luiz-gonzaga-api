namespace Modules.IdentityProvider.Application.ForgotPassword;

public sealed record ForgotPasswordResponse(string Email, string Token);
