using Shelter.Application.Abstractions.Messaging;

namespace Shelter.Application.PetSitters.SearchPetSitters;

public sealed record SearchPetSitterQuery(
    DateOnly StartDate,
    DateOnly EndDate) : IQuery<IReadOnlyList<PetSitterResponse>>;
