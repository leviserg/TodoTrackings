using Application.Commands;
using Domain.Entities;
using Infrastructure.Persistence;
using MediatR;

namespace Application.Handlers.RequestHandlers.CommandHandlers
{
    public class CreateTodoTaskCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateTodoTaskCommand, Guid>
    {
        private readonly IApplicationDbContext _context = context;
        public async Task<Guid> Handle(CreateTodoTaskCommand request, CancellationToken cancellationToken)
        {
            var todoItem = new TodoTask(request.Content, request.CreatedBy);
            _context.TodoTasks.Add(todoItem);
            await _context.SaveChangesAsync(cancellationToken);
            return todoItem.Id;
        }
    }
}
