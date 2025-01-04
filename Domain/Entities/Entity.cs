﻿using Domain.Contracts;
using MediatR;

namespace Domain.Entities
{
    public abstract class Entity : IHasDomainEvent
    {
        private readonly List<INotification> _domainEvents = new();
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();
        protected void AddDomainEvent(INotification eventItem)
        {
            _domainEvents.Add(eventItem);
        }
        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
