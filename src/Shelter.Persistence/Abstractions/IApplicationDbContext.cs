using Microsoft.EntityFrameworkCore;

namespace Shelter.Persistence.Abstractions;

public interface IApplicationDbContext
{
    public DbSet<TEntity> Set<TEntity>() where TEntity : class;
}
