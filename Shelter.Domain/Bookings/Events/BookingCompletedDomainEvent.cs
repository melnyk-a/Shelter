using Shelter.Domain.Abstractions;

namespace Shelter.Domain.Bookings.Events;

public sealed record BookingCompletedDomainEvent(Guid BookingId) : IDomainEvent;