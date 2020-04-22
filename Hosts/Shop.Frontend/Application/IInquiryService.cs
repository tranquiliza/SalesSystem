using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tranquiliza.Shop.Contract.Models;

namespace Shop.Frontend.Application
{
    public interface IInquiryService
    {
        List<InquiryModel> Inquiries { get; }
        Task Initialize();
        event Action OnChange;
    }
}
