using Application.Commands;
using Application.Exceptions;
using Domain.Entities;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.RequestHandlers.CommandHandlers
{
    public class CreateTodoTaskCommandHandler(IApplicationDbContext context, ILogger<CreateTodoTaskCommandHandler> logger) : IRequestHandler<CreateTodoTaskCommand, Guid>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly ILogger<CreateTodoTaskCommandHandler> _logger = logger;
        public async Task<Guid> Handle(CreateTodoTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var todoItem = new TodoTask(request.Content, request.CreatedBy);
                _context.TodoTasks.Add(todoItem);

                todoItem.AddTodoTaskEvent(todoItem);

                await _context.SaveChangesAsync(cancellationToken);
                return todoItem.Id;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while creating todo task: {Message}", e.Message);
                throw new UpdateDbContextException($"Could not create new TodoTask with content: '{request.Content}', created by '{request.CreatedBy}'.");
            }
        }
    }
}
