using Shelter.Domain.Users;

namespace Shelter.Auth.Keycloak.Authorization;

public sealed class UserRolesResponse
{
    public Guid Id { get; init; }
    public List<Role> Roles { get; init; } = [];
}
