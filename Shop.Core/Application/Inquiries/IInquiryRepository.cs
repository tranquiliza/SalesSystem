using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tranquiliza.Shop.Core.Model;

namespace Tranquiliza.Shop.Core.Application
{
    public interface IInquiryRepository
    {
        Task Save(Inquiry inquiry);
        Task<Inquiry> Get(Guid inquiryId);
        Task<IEnumerable<Inquiry>> GetInquiresFromClient(Guid clientId);
        Task<IEnumerable<Inquiry>> Get(InquiryState minimumState);
    }
}