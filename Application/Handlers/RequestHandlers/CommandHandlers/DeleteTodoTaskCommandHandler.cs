using Application.Commands;
using Application.Exceptions;
using Domain.Entities;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.RequestHandlers.CommandHandlers
{
    public class DeleteTodoTaskCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteTodoTaskCommand>
    {
        private readonly IApplicationDbContext _context = context;
        public async Task Handle(DeleteTodoTaskCommand request, CancellationToken cancellationToken)
        {
            var todoItem = await _context.TodoTasks
                .Where(t => t.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken)
                ?? throw new NotFoundException($"TodoTask with Id {request.Id} not found.");

            todoItem.DeleteTodoTask();

            _context.TodoTasks.Remove(todoItem);

            await _context.SaveChangesAsync(cancellationToken);

        }
    }
}
