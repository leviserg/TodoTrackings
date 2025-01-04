using MediatR;

namespace Domain.Events
{
    public record TodoTaskCreated(Guid Id, string Content, string CreatedBy) : INotification;

}
