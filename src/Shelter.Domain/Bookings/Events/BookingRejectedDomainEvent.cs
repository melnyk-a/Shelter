using Shelter.Domain.Abstractions;

namespace Shelter.Domain.Bookings.Events;

public sealed record BookingRejectedDomainEvent(Guid BookingId) : IDomainEvent;
