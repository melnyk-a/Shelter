using Shelter.Domain.Abstractions;

namespace Shelter.Domain.Reviews.Events;

public sealed record ReviewUpdatedDomainEvent(Guid ReviewId) : IDomainEvent;