using Microsoft.EntityFrameworkCore;
using Shelter.Domain.Abstractions;

namespace Shelter.Infrastructure.Repositories
{
    internal abstract class Repository<T>
        where T : Entity
    {
        protected Repository(ApplicationDbContext context)
        {
            DbContext = context;
        }

        protected ApplicationDbContext DbContext { get; }

        public async Task<T?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return await DbContext
                .Set<T>()
                .FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);
        }

        public virtual void Add(T entity)
        {
            DbContext.Add(entity);
        }
    }
}
