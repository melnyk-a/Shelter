using Shelter.Domain.Abstractions;

namespace Shelter.Domain.Bookings.Events;

public sealed record BookingConfirmedDomainEvent(Guid BookingId) : IDomainEvent;
