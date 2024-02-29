using Shelter.Domain.PetSitters;

namespace Shelter.Infrastructure.Repositories;

internal sealed class PetSitterRepository : Repository<PetSitter>, IPetSitterRepository
{
    public PetSitterRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
    }
}
