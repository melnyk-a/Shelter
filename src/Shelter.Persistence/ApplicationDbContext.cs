using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shelter.Application.Abstractions.Clock;
using Shelter.BackgroundJobs.Outbox;
using Shelter.Domain.Abstractions;
using Shelter.Domain.Abstractions.Persistence;
using Shelter.Persistence.Abstractions;

namespace Shelter.Persistence
{
    public sealed class ApplicationDbContext : DbContext, IUnitOfWork, IApplicationDbContext
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings = new()
        {
            TypeNameHandling = TypeNameHandling.All
        };

        private readonly IDateTimeProvider _dateTimeProvider;

        public ApplicationDbContext(DbContextOptions options,
            IDateTimeProvider dateTimeProvider)
            : base(options)
        {
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

                AddDomainEventsAsOutboxMessages();

                var result = await base.SaveChangesAsync(cancellationToken);

                return result;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new DbUpdateConcurrencyException("Concurrency exception occurred", ex);
            }
        }

        private void AddDomainEventsAsOutboxMessages()
        {
            var outboxMessages = ChangeTracker
                .Entries<Entity>()
                .Select(entry => entry.Entity)
                .SelectMany(entity =>
                {
                    var domainEvents = entity.GetDomainEvents();

                    entity.ClearDomainEvents();

                    return domainEvents;
                })
                .Select(domainEvent => new OutboxMessage(
                    Guid.NewGuid(),
                    _dateTimeProvider.UtcNow,
                    domainEvent.GetType().Name,
                    JsonConvert.SerializeObject(domainEvent, JsonSerializerSettings)))
                .ToList();

            AddRange(outboxMessages);
        }
    }
}
