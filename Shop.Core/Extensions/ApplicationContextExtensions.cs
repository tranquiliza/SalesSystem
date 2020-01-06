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

        public static bool HasAccessTo(this IApplicationContext context, CustomerInformation customerInformation)
        {
            if (context.IsAnonymous && customerInformation.UserId == default)
                return true;

            if (!context.IsAnonymous)
            {
                if (customerInformation.UserId != default && context.UserId == customerInformation.UserId)
                    return true;
                else if (customerInformation.UserId == default)
                    return true;
            }

            return false;
        }

        public static bool IsAdmin(this IApplicationContext context)
        {
            return context.User?.HasRole("ADMIN") == true;
        }
    }
}
