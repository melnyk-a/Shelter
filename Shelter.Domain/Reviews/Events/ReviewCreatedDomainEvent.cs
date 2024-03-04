﻿using Shelter.Domain.Abstractions;

namespace Shelter.Domain.Reviews.Events;
public sealed record ReviewCreatedDomainEvent(Guid ReviewId) : IDomainEvent;