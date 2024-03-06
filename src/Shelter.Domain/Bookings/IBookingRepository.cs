using Shelter.Domain.Abstractions.Persistence;
using Shelter.Domain.PetSitters;

namespace Shelter.Domain.Bookings;

public interface IBookingRepository : IRepository<Booking>
{
    Task<bool> IsOverlappingAsync(
        PetSitter petSitter,
        DateRange duration,
        CancellationToken cancellationToken = default);
}
