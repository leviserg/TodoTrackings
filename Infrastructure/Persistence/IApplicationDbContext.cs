using Domain.Contracts;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{


    public interface IApplicationDbContext
    {
        public DbSet<TodoTask> TodoTasks { get; set; }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}
