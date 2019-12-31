using System;
using System.Collections.Generic;
using System.Text;

namespace Tranquiliza.Shop.Contract.Models
{
    public class InquiryModel
    {
        public Guid Id { get; set; }
        public List<OrderLineModel> OrderLines { get; set; }
        public InquiryStateModel State { get; set; }
        public CustomerInformationModel Customer { get; set; }
        public double Total { get; set; }
    }
}
