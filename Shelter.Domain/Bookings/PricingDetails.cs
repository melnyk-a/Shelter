using Shelter.Domain.Shared;

namespace Shelter.Domain.Bookings;

public sealed record PricingDetails(
    Money PricingForPeriod,
    Money SecurityDeposit,
    Money AmenititiesUpCharge,
    Money TotalPrice);
