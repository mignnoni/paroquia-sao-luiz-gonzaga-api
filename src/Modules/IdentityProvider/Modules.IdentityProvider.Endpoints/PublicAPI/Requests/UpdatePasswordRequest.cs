namespace Modules.IdentityProvider.Endpoints.PublicAPI.Requests;

public sealed record UpdatePasswordRequest(
    Guid UserId, 
    string CurrentPassword, 
    string NewPassword, 
    string ConfirmNewPassword);