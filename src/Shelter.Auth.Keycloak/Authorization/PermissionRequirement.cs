using Microsoft.AspNetCore.Authorization;

namespace Shelter.Auth.Keycloak.Authorization;

internal sealed class PermissionRequirement: IAuthorizationRequirement
{
    public PermissionRequirement(string permission)
    {
        Permission = permission;
    }

    public string Permission { get; }
}
