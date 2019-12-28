using System;
using System.Collections.Generic;
using System.Text;
using Tranquiliza.Shop.Core.Model;

namespace Tranquiliza.Shop.Core.Extensions
{
    public static class ApplicationContextExtensions
    {
        public static bool HasAccessTo(this IApplicationContext context, Inquiry inquiry)
            => (context.User != null && inquiry.UserId == context.User.Id) || inquiry.CreatedByClient == context.ClientId;

        // TODO: Refactor IApplicationContext to have user instead of just UserId.
        public static bool HasAccessTo(this IApplicationContext context, CustomerInformation customerInformation)
            => context.User?.Id == customerInformation.UserId
            || (customerInformation.UserId == default && !context.IsAnonymous);
    }
}
