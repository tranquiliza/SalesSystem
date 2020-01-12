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

        public static bool IsAdmin(this IApplicationContext context)
        {
            return context.User?.HasRole("ADMIN") == true;
        }
    }
}
