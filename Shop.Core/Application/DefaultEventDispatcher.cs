using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tranquiliza.Shop.Core.Model;

namespace Tranquiliza.Shop.Core.Application
{
    public class DefaultEventDispatcher : IEventDispatcher
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMediator _mediator;
        private readonly ILogger _log;

        public DefaultEventDispatcher(IEventRepository eventRepository, IMediator mediator, ILogger log)
        {
            _eventRepository = eventRepository;
            _mediator = mediator;
            _log = log;
        }

        public async Task DispatchEvents(DomainEntityBase domainEntitiy)
        {
            _log.Info($"Persisting DomainEvents for {domainEntitiy.GetType().Name}");

            var domainEvents = domainEntitiy.DomainEvents;
            await _eventRepository.Save(domainEvents).ConfigureAwait(false);

            foreach (var domainEvent in domainEvents)
                await _mediator.Publish(domainEvent).ConfigureAwait(false);
        }
    }
}
