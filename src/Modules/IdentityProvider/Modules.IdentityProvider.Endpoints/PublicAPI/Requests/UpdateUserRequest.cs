namespace Modules.IdentityProvider.Endpoints.PublicAPI.Requests;
public sealed record UpdateUserRequest(
    Guid CurrentUserId,
    Guid UserId,
    string Name,
    string OldRole,
    string? NewRole = null);