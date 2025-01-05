using Domain.Entities;
using MediatR;

namespace Domain.Events
{
    public record TodoTaskUpdated(Guid Id, TodoTask TodoTaskItem) : INotification;

}
