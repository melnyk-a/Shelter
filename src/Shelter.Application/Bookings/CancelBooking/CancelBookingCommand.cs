using Shelter.Application.Abstractions.Messaging;

namespace Shelter.Application.Bookings.CancelBooking;

public record CancelBookingCommand(Guid BookingId) : ICommand;