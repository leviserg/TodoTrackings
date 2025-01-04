using Application.DTOs;
using Application.Exceptions;
using Application.Helpers;
using Application.Queries;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.RequestHandlers.QueryHandlers
{
    public class GetTodoTaskByIdQueryHandler(IApplicationDbContext context) : IRequestHandler<GetTodoTaskByIdQuery, TodoTaskDto>
    {

        private readonly IApplicationDbContext _context = context;
        public async Task<TodoTaskDto> Handle(GetTodoTaskByIdQuery request, CancellationToken cancellationToken)
        {
            var task = await _context.TodoTasks
                .Where(t => t.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken)
                ?? throw new NotFoundException($"TodoTask with Id {request.Id} not found.");

            return TodoTaskMapper.TodoTaskToDto(task);
        }
    }
}
