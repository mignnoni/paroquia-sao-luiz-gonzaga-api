namespace Modules.IdentityProvider.Application.Common;

public sealed record ResetPasswordToken(
    string Token, 
    string Email);
