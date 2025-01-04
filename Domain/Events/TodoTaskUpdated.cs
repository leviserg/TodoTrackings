using MediatR;

namespace Domain.Events
{
    public record TodoTaskUpdated(Guid Id, string TaskContent, bool IsCompleted) : INotification;

}
