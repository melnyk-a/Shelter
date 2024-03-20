using Shelter.Domain.PetSitters;
using Shelter.Domain.Shared;

namespace Shelter.Domain.UnitTests.PetSitters;

internal static class PetSitterData
{
    public static PetSitter Create(Money price, Money? securityDeposit = null) => new(
        Guid.NewGuid(),
        new Name("Test petSitter"),
        new Description("Test description"),
        new Address("Country", "State", "City", "Street"),
        price,
        securityDeposit ?? Money.Zero(),
        [],
        []);
}
