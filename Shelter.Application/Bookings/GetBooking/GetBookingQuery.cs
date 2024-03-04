using Shelter.Application.Abstractions.Messaging;

namespace Shelter.Application.Bookings.GetBooking;

public sealed record GetBookingQuery(Guid BookingId) : IQuery<BookingResponse>;
