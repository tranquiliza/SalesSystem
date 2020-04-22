using Shop.Frontend.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tranquiliza.Shop.Contract.Models;

namespace Shop.Frontend.Application
{
    public class InquiryService : IInquiryService
    {
        private readonly IApiGateway apiGateway;
        public List<InquiryModel> Inquiries { get; private set; } = new List<InquiryModel>();

        public event Action OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();

        public InquiryService(IApiGateway apiGateway)
        {
            this.apiGateway = apiGateway;
        }

        public async Task Initialize()
        {
            var minimumState = new QueryParam("inquiryState", nameof(InquiryStateModel.Placed));
            Inquiries = await apiGateway.Get<List<InquiryModel>>("Inquiries", queryParams: minimumState).ConfigureAwait(false);

            NotifyStateChanged();
        }
    }
}
