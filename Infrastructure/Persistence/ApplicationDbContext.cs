using Domain.Contracts;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IMediator mediator) : DbContext(options), IApplicationDbContext
    {

        private readonly IMediator _mediator = mediator;

        public DbSet<TodoTask> TodoTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
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
        }
    }
}
