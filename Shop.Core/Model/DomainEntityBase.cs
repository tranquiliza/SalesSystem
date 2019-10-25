using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tranquiliza.Shop.Core.Model
{
    public abstract class DomainEntityBase
    {
        private readonly List<INotification> _domainEvents = new List<INotification>();

        public IReadOnlyList<INotification> DomainEvents => _domainEvents.AsReadOnly();

        protected void AddEvent(INotification domainEvent) => _domainEvents.Add(domainEvent);
    }
}
