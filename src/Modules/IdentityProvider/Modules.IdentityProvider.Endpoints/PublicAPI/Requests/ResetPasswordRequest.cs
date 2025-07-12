namespace Modules.IdentityProvider.Endpoints.PublicAPI.Requests;

public sealed record ResetPasswordRequest(
    string Email, 
    string Token, 
    string NewPassword, 
    string ConfirmNewPassword);