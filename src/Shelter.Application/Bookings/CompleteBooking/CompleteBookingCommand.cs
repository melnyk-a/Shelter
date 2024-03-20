using Shelter.Application.Abstractions.Messaging;

namespace Shelter.Application.Bookings.CompleteBooking;

public record CompleteBookingCommand(Guid BookingId) : ICommand;