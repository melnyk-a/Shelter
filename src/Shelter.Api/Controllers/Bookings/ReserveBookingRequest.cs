namespace Shelter.Api.Controllers.Bookings
{
    public sealed record ReserveBookingRequest(
        Guid PetSitterId,
        Guid UserId,
        DateOnly StartDate,
        DateOnly EndDate);
}