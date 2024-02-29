using Shelter.Domain.Abstractions;

namespace Shelter.Domain.Users.Events;

public sealed record UserCreatedDomainEvent(Guid UserId) : IDomainEvent;
