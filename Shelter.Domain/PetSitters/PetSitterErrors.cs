using Shelter.Domain.Abstractions;

namespace Shelter.Domain.PetSitters;

public static class PetSitterErrors
{
    public static Error NotFound = new(
        "PetSitter.NotFound",
        "The pet sitter with the specified identifier was not found");
}
