using Shelter.Application.Abstractions.Messaging;
using Shelter.Application.PetSitters.Shared;

namespace Shelter.Application.PetSitters.GetPetSitters;

public sealed record GetPetSittersListQuery() : IQuery<IReadOnlyList<PetSitterResponse>>;
