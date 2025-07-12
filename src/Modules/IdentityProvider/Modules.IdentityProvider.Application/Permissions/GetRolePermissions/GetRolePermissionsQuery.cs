using BuildingBlocks.Application;

namespace Modules.IdentityProvider.Application.Permissions.GetRolePermissions;

public sealed record GetRolePermissionsQuery(string Role) : IQuery<HashSet<string>>
{
}
