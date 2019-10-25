using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tranquiliza.Shop.Core.Model;

namespace Tranquiliza.Shop.Core.Application
{
    public interface IEventDispatcher
    {
        public Task DispatchEvents(DomainEntityBase domainEntitiy);
    }
}
