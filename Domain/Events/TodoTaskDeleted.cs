using MediatR;

namespace Domain.Events
{
    public record TodoTaskDeleted(Guid Id) : INotification;

}
