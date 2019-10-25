using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;

namespace Tranquiliza.Shop.Core.Application
{
    public interface IEventRepository
    {
        Task Save(IReadOnlyList<INotification> domainEvents);
    }
}