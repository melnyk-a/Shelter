namespace Shelter.Domain.PetSitters;

public interface IPetSitterRepository
{
    Task<PetSitter> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
