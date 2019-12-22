using System;
using System.Collections.Generic;
using System.Text;
using Tranquiliza.Shop.Core.Model;

namespace Tranquiliza.Shop.Core.Extensions
{
    public static class ApplicationContextExtensions
    {
        public static bool HasAccessTo(this IApplicationContext context, Inquiry inquiry) => (context.UserId != default && inquiry.UserId == context.UserId) || inquiry.CreatedByClient == context.ClientId;
    }
}
