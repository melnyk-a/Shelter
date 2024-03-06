using Shelter.Application.Abstractions.Messaging;
using Shelter.Application.PetSitters.Shared;

namespace Shelter.Application.PetSitters.SearchPetSitters;

public sealed record SearchPetSitterQuery(
    DateOnly StartDate,
    DateOnly EndDate) : IQuery<IReadOnlyList<PetSitterResponse>>;
