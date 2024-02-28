using Shelter.Domain.Abstractions;

namespace Shelter.Domain.Bookings.Events;

public sealed record BookingCanceledDomainEvent(Guid BookingId) : IDomainEvent;
