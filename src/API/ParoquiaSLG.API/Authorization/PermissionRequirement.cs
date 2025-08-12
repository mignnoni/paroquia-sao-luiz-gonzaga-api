using Microsoft.AspNetCore.Authorization;

namespace ParoquiaSLG.API.Authorization;

internal sealed class PermissionRequirement : IAuthorizationRequirement
{
    internal PermissionRequirement(string permission) => Permission = permission;
    internal string Permission { get; }
}
