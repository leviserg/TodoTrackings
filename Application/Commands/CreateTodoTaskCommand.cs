using MediatR;

namespace Application.Commands
{
    public class CreateTodoTaskCommand : IRequest<Guid>
    {
        public required string Content { get; set; }
        public required string CreatedBy { get; set; }
    }
}