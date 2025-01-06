using MediatR;

namespace Domain.Contracts
{
    public interface IHasDomainEvent
    {
        IReadOnlyCollection<INotification> DomainEvents { get; }
        void AddDomainEvent(INotification eventItem);
        void ClearDomainEvents();
    }
}
