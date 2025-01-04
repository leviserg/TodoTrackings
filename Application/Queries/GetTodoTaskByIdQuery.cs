using Application.DTOs;
using MediatR;

namespace Application.Queries
{
    public class GetTodoTaskByIdQuery : IRequest<TodoTaskDto>
    {
        public Guid Id { get; set; }
    }
}
