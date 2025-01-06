using Application.Commands;
using Application.Exceptions;
using Domain.Entities;
using Domain.Events;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.RequestHandlers.CommandHandlers
{
    public class DeleteTodoTaskCommandHandler(
        IApplicationDbContext context,
        ILogger<DeleteTodoTaskCommandHandler> logger,
        DeletedEntity deletedEntity) : IRequestHandler<DeleteTodoTaskCommand>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly ILogger<DeleteTodoTaskCommandHandler> _logger = logger;
        public async Task Handle(DeleteTodoTaskCommand request, CancellationToken cancellationToken)
        {
            var todoItem = await _context.TodoTasks
                .Where(t => t.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken)
                ?? throw new NotFoundException($"TodoTask with Id {request.Id} not found.");

            try
            {
                deletedEntity.AddDomainEvent(new TodoTaskDeleted(Guid.NewGuid(), todoItem));

                _context.TodoTasks.Remove(todoItem);

                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while deleting todo task with Id {TodoTaskId}: {Message}", todoItem.Id, e.Message);
                throw new UpdateDbContextException($"Could not delete TodoTask with Id {todoItem.Id}.");
            }


        }
    }
}
