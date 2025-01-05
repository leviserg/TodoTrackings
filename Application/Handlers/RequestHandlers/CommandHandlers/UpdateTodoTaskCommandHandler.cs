using Application.Commands;
using Application.DTOs;
using Application.Exceptions;
using Application.Helpers;
using Domain.Entities;
using Infrastructure.Persistence;
using MassTransit.Transports;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.Handlers.RequestHandlers.CommandHandlers
{
    public class UpdateTodoTaskCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateTodoTaskCommand, TodoTaskDto>
    {
        private readonly IApplicationDbContext _context = context;

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
            catch (DbUpdateConcurrencyException)
            {
                if (!(await _context.TodoTasks.AnyAsync(e => e.Id == request.Id, cancellationToken)))
                {
                    return new TodoTaskDto();
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
