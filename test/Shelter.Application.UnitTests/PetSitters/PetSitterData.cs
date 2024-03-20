using Shelter.Domain.PetSitters;
using Shelter.Domain.Shared;

namespace Shelter.Application.UnitTests.PetSitters;

internal static class PetSitterData
{
    public static PetSitter Create() => new(
        Guid.NewGuid(),
        new Name("Test petSitter"),
        new Description("Test description"),
        new Address("Country", "State", "City", "Street"),
        new Money(100.0m, Currency.Usd),
        Money.Zero(),
        [],
        []);
}
