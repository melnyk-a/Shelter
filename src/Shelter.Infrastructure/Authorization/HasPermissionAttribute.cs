using Microsoft.AspNetCore.Authorization;

namespace Shelter.Infrastructure.Authorization;

public sealed class HasPermissionAttribute: AuthorizeAttribute
{
    public HasPermissionAttribute(string permission)
        : base(permission)
    {
    }
}
