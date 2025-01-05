using Domain.Contracts;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    /* install packages :
       Infrastructure project:
        - Microsoft.EntityFrameworkCore;
	    - Microsoft.EntityFrameworkCore.SqlServer;
	    - Microsoft.EntityFrameworkCore.Design;
    Presentation project:
        - Microsoft.EntityFrameworkCore.Tools;

    $ add migration > dotnet ef migrations add InitialMigration --project Infrastructure --startup-project Presentation
    $ apply migr.   > dotnet ef database update --project Infrastructure --startup-project Presentation
    $ drop db.      > dotnet ef database drop --project Infrastructure --startup-project Presentation
    $ remove migr.  > dotnet ef migrations remove --project Infrastructure --startup-project Presentation
    */
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IMediator mediator) : DbContext(options), IApplicationDbContext
    {

        private readonly IMediator _mediator = mediator;

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
        }
    }
}
