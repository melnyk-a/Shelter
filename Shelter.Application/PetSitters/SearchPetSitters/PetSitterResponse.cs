namespace Shelter.Application.PetSitters.SearchPetSitters;

public sealed class PetSitterResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public decimal Price { get; init; }
    public string Currency { get; init; }
    public AddressResponse Address { get; set; }
}
