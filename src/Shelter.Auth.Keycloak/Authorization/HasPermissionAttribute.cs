using Microsoft.AspNetCore.Authorization;

namespace Shelter.Auth.Keycloak.Authorization;

public sealed class HasPermissionAttribute: AuthorizeAttribute
{
    public HasPermissionAttribute(string permission)
        : base(permission)
    {
    }
}
