using System;
using System.Threading.Tasks;
using Tranquiliza.Shop.Core.Model;

namespace Tranquiliza.Shop.Core.Application
{
    public interface IInquiryRepository
    {
        Task Save(Inquiry inquiry);
        Task<Inquiry> GetInquiry(Guid inquiryId);
    }
}