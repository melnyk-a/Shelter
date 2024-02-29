namespace Shelter.Domain.PetSitters;

public sealed record Address(
    string Country,
    string State,
    string City,
    string Street);

