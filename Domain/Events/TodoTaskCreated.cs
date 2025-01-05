using Domain.Entities;
using MediatR;

namespace Domain.Events
{
    public record TodoTaskCreated(Guid Id, TodoTask TodoTaskItem) : INotification;

}
