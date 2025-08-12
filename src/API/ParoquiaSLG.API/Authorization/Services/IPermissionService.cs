namespace ParoquiaSLG.API.Authorization.Services;

internal interface IPermissionService
{
    Task<HashSet<string>> GetPermissionsByRoleAsync(string identityProviderId, CancellationToken cancellationToken = default);
}
