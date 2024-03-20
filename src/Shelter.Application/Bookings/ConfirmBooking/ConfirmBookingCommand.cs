using Shelter.Application.Abstractions.Messaging;

namespace Shelter.Application.Bookings.ConfirmBooking;

public sealed record ConfirmBookingCommand(Guid BookingId) : ICommand;