using Shelter.Domain.Abstractions;
using Shelter.Domain.Shared;

namespace Shelter.Domain.PetSitters;

public sealed class PetSitter : Entity
{
    public PetSitter(
        Guid id,
        Name name,
        Description description,
        Address address,
        Money price,
        Money securityDeposit,
        List<PetSize> petSizes,
        List<PetTypes> petTypes)
        : base(id)
    {
        Name = name;
        Description = description;
        Address = address;
        Price = price;
        SecurityDeposit = securityDeposit;
        AllowedPetSizes = petSizes;
        AllowedPetTypes = petTypes;
    }

    private PetSitter()
    {
    }

    public Name Name { get; private set; }
    public Description Description { get; private set; }
    public Address Address { get; private set; }
    public Money Price { get; private set; }
    public Money SecurityDeposit { get; private set; }
    public DateTime? LastBookedOnUtc { get; internal set; }
    public List<PetSize> AllowedPetSizes { get; private set; }
    public List<PetTypes> AllowedPetTypes { get; private set; }
}
