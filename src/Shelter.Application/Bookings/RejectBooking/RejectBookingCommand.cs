using Shelter.Application.Abstractions.Messaging;

namespace Shelter.Application.Bookings.RejectBooking;

public sealed record RejectBookingCommand(Guid BookingId) : ICommand;