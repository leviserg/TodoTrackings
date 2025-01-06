using Domain.Contracts;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    /* install packages :

    */
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IMediator mediator, DeletedEntity deletedEntity) : DbContext(options), IApplicationDbContext
    {

        private readonly IMediator _mediator = mediator;
        private readonly DeletedEntity _deletedEntity = deletedEntity;
        public DbSet<TodoTask> TodoTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Ignore<List<INotification>>()
                .ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            modelBuilder.Entity<TodoTask>(entity =>
            {
                entity.Property(e => e.Content)
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .IsRequired();

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime");
            });
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var result = await base.SaveChangesAsync(cancellationToken);

            await DispatchDomainEventsAsync();

            return result;
        }


        private async Task DispatchDomainEventsAsync()
        {
            var entitiesWithEvents = ChangeTracker
                .Entries<IHasDomainEvent>()
                .Where(entry => entry.Entity.DomainEvents.Count > 0)
                .Select(entry => entry.Entity)
                .ToList();

            foreach (var entity in entitiesWithEvents)
            {
                var domainEvents = entity.DomainEvents.ToList();
                entity.ClearDomainEvents();

                foreach (var domainEvent in domainEvents)
                {
                    await _mediator.Publish(domainEvent);
                }
            }

            await DispatchDomainEventsForDeletedEntityAsync();
        }

        private async Task DispatchDomainEventsForDeletedEntityAsync()
        {
            if (_deletedEntity.DomainEvents.Count == 0) return;
            var deletedEvents = _deletedEntity.DomainEvents;

            foreach (var deletedEvent in deletedEvents)
            {
                await _mediator.Publish(deletedEvent);
            }

            _deletedEntity.ClearDomainEvents();
        }

    }
}
