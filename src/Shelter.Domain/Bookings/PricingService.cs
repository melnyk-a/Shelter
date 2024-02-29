using Shelter.Domain.PetSitters;
using Shelter.Domain.Shared;

namespace Shelter.Domain.Bookings;

public sealed class PricingService
{
    public PricingDetails CalculatePrice(PetSitter petSitter, DateRange period)
    {
        var currency = petSitter.Price.Currency;

        var priceForPeriod = new Money(
            petSitter.Price.Amount * period.LengthInDays,
            currency);

        decimal percentageUpCharge = 0;
        foreach (var size in petSitter.AllowedPetSizes)
        {
            percentageUpCharge += size switch
            {
                PetSize.Over40kg => 0.05m,
                PetSize.From20To40kg => 0.01m,
                _ => 0
            };
        }

        foreach (var type in petSitter.AllowedPetTypes)
        {
            percentageUpCharge += type switch
            {
                PetTypes.Rabbits or PetTypes.GuineaPigs => 0.05m,
                PetTypes.Birds => 0.01m,
                PetTypes.Reptiles => 0.08m,
                _ => 0
            };
        }

        var amelitiesUpCharge = Money.Zero(currency);
        if (percentageUpCharge > 0)
        {
            amelitiesUpCharge = new Money(
                priceForPeriod.Amount * percentageUpCharge, currency);
        }

        var totalPrice = Money.Zero();
        totalPrice += priceForPeriod;

        if (petSitter.SecurityDeposit.IsZero())
        {
            totalPrice += petSitter.SecurityDeposit;
        }

        totalPrice += amelitiesUpCharge;

        return new PricingDetails(
            priceForPeriod,
            petSitter.SecurityDeposit,
            amelitiesUpCharge,
            totalPrice);
    }
}
