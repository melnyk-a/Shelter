using MediatR;
using Microsoft.EntityFrameworkCore;
using Shelter.Application.Abstractions.Clock;
using Shelter.Domain.Abstractions;
using Shelter.Domain.Abstractions.Persistence;
using Shelter.Persistence.Abstractions;

namespace Shelter.Persistence
{
    public sealed class ApplicationDbContext : DbContext, IUnitOfWork, IApplicationDbContext
    {
        private readonly IPublisher _publisher;
        private readonly IDateTimeProvider _dateTimeProvider;

        public ApplicationDbContext(DbContextOptions options,
            IPublisher publisher,
            IDateTimeProvider dateTimeProvider)
            : base(options)
        {
            _publisher = publisher;
            _dateTimeProvider = dateTimeProvider;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                foreach (var entity in ChangeTracker.Entries<AuditableEntity>())
                {
                    switch (entity.State)
                    {
                        case EntityState.Added:
                            entity.Entity.CreatedDate = _dateTimeProvider.UtcNow;
                            break;
                        case EntityState.Modified:
                            entity.Entity.LastModifiedDate = _dateTimeProvider.UtcNow;
                            break;
                    }
                }

                var result = await base.SaveChangesAsync(cancellationToken);

                await PublishDomainEventsAsync();

                return result;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new DbUpdateConcurrencyException("Concurrency exception occurred", ex);
            }
        }

        private async Task PublishDomainEventsAsync()
        {
            var domainEvents = ChangeTracker
                .Entries<Entity>()
                .Select(entity => entity.Entity)
                .SelectMany(entity =>
                {
                    var domainEvents = entity.GetDomainEvents();

                    entity.ClearDomainEvents();
                    return domainEvents;
                })
                .ToList();

            foreach (var domainEvent in domainEvents)
            {
                await _publisher.Publish(domainEvent);
            }
        }
    }
}
