using FluentValidation;

namespace Shelter.Application.Bookings.ReserveBooking;

public sealed class ReserveBookingCommandValidator: AbstractValidator<ReserveBookingCommand>
{
    public ReserveBookingCommandValidator()
    {
        RuleFor(c => c.UserId).NotEmpty();

        RuleFor(c => c.PetSitterId).NotEmpty();

        RuleFor(c => c.StartDate).LessThan(c => c.EndDate);
    }
}
