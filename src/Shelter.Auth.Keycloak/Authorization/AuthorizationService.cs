using Microsoft.EntityFrameworkCore;
using Shelter.Domain.Users;
using Shelter.Persistence.Abstractions;

namespace Shelter.Auth.Keycloak.Authorization;

internal sealed class AuthorizationService
{
    private readonly IApplicationDbContext _dbContext;

    public AuthorizationService(IApplicationDbContext applicationDbContext)
    {
        _dbContext = applicationDbContext;
    }

    public async Task<UserRolesResponse> GetRolesForUserAsync(string identityId)
    {
        var result = await _dbContext.Set<User>()
            .Where(user => user.IdentityId == identityId)
            .Select(user => new UserRolesResponse
            {
                Id = user.Id,
                Roles = user.Roles.ToList()
            })
            .FirstAsync();

        return result;
    }

    public async Task<HashSet<string>> GetPermissionsForUserAsync(string identityId)
    {
        var permisisons = await _dbContext.Set<User>()
            .Where(user => user.IdentityId == identityId)
            .SelectMany(user => user.Roles.Select(role => role.Permissions))
            .FirstAsync();

        var permisisonsSet = permisisons.Select(x => x.Name).ToHashSet();

        return permisisonsSet;
    }
}
