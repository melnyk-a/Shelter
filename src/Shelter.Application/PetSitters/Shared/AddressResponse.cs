namespace Shelter.Application.PetSitters.Shared;

public sealed class AddressResponse
{
    public string Country { get; init; }
    public string State { get; init; }
    public string City { get; init; }
    public string Street { get; init; }
}