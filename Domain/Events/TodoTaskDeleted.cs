using Domain.Entities;
using MediatR;

namespace Domain.Events
{
    public record TodoTaskDeleted(Guid Id, TodoTask TodoTaskItem) : INotification;

}
