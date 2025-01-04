using MediatR;

namespace Application.Commands
{
    public class DeleteTodoTaskCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
