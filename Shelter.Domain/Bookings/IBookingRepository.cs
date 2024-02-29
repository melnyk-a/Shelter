using Shelter.Domain.PetSitters;

namespace Shelter.Domain.Bookings;

public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> IsOverlappingAsync(
        PetSitter petSitter,
        DateRange duration,
        CancellationToken cancellationToken = default);

    void Add(Booking booking);
}
