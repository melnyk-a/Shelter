using Shelter.Application.Abstractions.Messaging;

namespace Shelter.Application.Bookings.ReserveBooking;

public record class ReserveBookingCommand(
    Guid PetSitterId,
    Guid UserId,
    DateOnly StartDate,
    DateOnly EndDate) : ICommand<Guid>;

