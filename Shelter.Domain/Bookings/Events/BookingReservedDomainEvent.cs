using Shelter.Domain.Abstractions;

namespace Shelter.Domain.Bookings.Events;

public sealed record BookingReservedDomainEvent(Guid BookingId) : IDomainEvent;
