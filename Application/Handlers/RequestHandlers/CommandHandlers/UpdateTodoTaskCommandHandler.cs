using Application.Commands;
using Application.DTOs;
using Application.Exceptions;
using Application.Helpers;
using Domain.Entities;
using Infrastructure.Persistence;
using MassTransit.Transports;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Application.Handlers.RequestHandlers.CommandHandlers
{
    public class UpdateTodoTaskCommandHandler(IApplicationDbContext context, ILogger<UpdateTodoTaskCommandHandler> logger) : IRequestHandler<UpdateTodoTaskCommand, TodoTaskDto>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly ILogger<UpdateTodoTaskCommandHandler> _logger = logger;

        public async Task<TodoTaskDto> Handle(UpdateTodoTaskCommand request, CancellationToken cancellationToken)
        {
            var todoItem = await _context.TodoTasks
                .Where(t => t.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken)
                ?? throw new NotFoundException($"TodoTask with Id {request.Id} not found.");


            todoItem.UpdateTodoTaskEvent(todoItem, request.Content, request.IsCompleted);

            _context.TodoTasks.Update(todoItem);

            try
            {
                await _context.SaveChangesAsync(cancellationToken);

                return TodoTaskMapper.TodoTaskToDto(todoItem);
            }
            catch (DbUpdateConcurrencyException e)
            {
                _logger.LogError(e, "An error occurred while updating todo task with Id {TodoTaskId}: {Message}", todoItem.Id, e.Message);
                throw new UpdateDbContextException($"Could not update TodoTask with Id {todoItem.Id}.");
            }
        }
    }
}
