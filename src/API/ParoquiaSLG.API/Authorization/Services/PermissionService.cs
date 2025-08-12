using Microsoft.Extensions.Caching.Memory;
using Modules.IdentityProvider.Endpoints.PublicAPI;

namespace ParoquiaSLG.API.Authorization.Services;

internal sealed class PermissionService(IMemoryCache memoryCache, IIdentityProviderEndpoints userAccessEndpoints) : IPermissionService
{
    private readonly IMemoryCache _memoryCache = memoryCache;
    private readonly IIdentityProviderEndpoints _userAccessEndpoints = userAccessEndpoints;

    /// <inheritdoc />
    public async Task<HashSet<string>> GetPermissionsByRoleAsync(string role, CancellationToken cancellationToken = default) =>
        await _memoryCache.GetOrCreateAsync(
            CreateCacheKey(role),
            async cacheEntry =>
            {
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1);

                return await GetPermissionsByRoleInternalAsync(role, cancellationToken);
            });

    private static string CreateCacheKey(string role) => $"permissions_{role}";

    private async Task<HashSet<string>> GetPermissionsByRoleInternalAsync(string role, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(role))
        {
            return [];
        }

        var result = await _userAccessEndpoints.GetPermissionsByRole(role, cancellationToken);

        return result.Value;
    }

}
