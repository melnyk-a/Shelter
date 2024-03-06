using Microsoft.EntityFrameworkCore;
using Shelter.Domain.Users;

namespace Shelter.Persistence.Repositories;
internal sealed class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
    }

    public override void Add(User entity)
    {
        foreach (var role in entity.Roles)
        {
            DbContext.Attach(role);
        }

        base.Add(entity);
    }

    public override async Task AddAsync(User entity, CancellationToken cancellationToken = default)
    {
        foreach (var role in entity.Roles)
        {
            DbContext.Attach(role);
        }

        await base.AddAsync(entity, cancellationToken);
    }
}
