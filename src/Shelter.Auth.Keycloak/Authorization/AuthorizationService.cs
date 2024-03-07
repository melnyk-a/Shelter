using Microsoft.EntityFrameworkCore;
using Shelter.Application.Abstractions.Caching;
using Shelter.Domain.Users;
using Shelter.Persistence.Abstractions;

namespace Shelter.Auth.Keycloak.Authorization;

internal sealed class AuthorizationService
{
    private readonly IApplicationDbContext _dbContext;
    private readonly ICacheService _cacheService;

    public AuthorizationService(
        IApplicationDbContext applicationDbContext,
        ICacheService cacheService)
    {
        _dbContext = applicationDbContext;
        _cacheService = cacheService;
    }

    public async Task<UserRolesResponse> GetRolesForUserAsync(string identityId)
    {
        var cacheKey = $"auth:roles-{identityId}";

        var cachedRoles = await _cacheService.GetAsync<UserRolesResponse>(cacheKey);

        if (cachedRoles is not null)
        {
            return cachedRoles;
        }

        var result = await _dbContext.Set<User>()
            .Where(user => user.IdentityId == identityId)
            .Select(user => new UserRolesResponse
            {
                Id = user.Id,
                Roles = user.Roles.ToList()
            })
            .FirstAsync();

        await _cacheService.SetAsync(cacheKey, result);

        return result;
    }

    public async Task<HashSet<string>> GetPermissionsForUserAsync(string identityId)
    {
        var cacheKey = $"auth:permissions-{identityId}";

        var cachedPermissions = await _cacheService.GetAsync<HashSet<string>>(cacheKey);

        if (cachedPermissions is not null)
        {
            return cachedPermissions;
        }

        var permisisons = await _dbContext.Set<User>()
            .Where(user => user.IdentityId == identityId)
            .SelectMany(user => user.Roles.Select(role => role.Permissions))
            .FirstAsync();

        var permisisonsSet = permisisons.Select(x => x.Name).ToHashSet();

        await _cacheService.SetAsync(cacheKey, permisisonsSet);

        return permisisonsSet;
    }
}
