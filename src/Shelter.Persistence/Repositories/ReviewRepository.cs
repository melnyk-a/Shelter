using Shelter.Domain.PetSitters;
using Shelter.Domain.Reviews;

namespace Shelter.Persistence.Repositories;

internal sealed class ReviewRepository : Repository<Review>, IReviewRepository
{
    public ReviewRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
    }
}
