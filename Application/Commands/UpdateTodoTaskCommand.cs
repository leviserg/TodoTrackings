using Application.DTOs;
using MediatR;

namespace Application.Commands
{
    public class UpdateTodoTaskCommand : IRequest<TodoTaskDto>
    {
        public Guid Id { get; set; }
        public string? Content { get; set; }
        public bool IsCompleted { get; set; }
    }
}
