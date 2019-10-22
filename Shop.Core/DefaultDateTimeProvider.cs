using System;
using System.Collections.Generic;
using System.Text;

namespace Tranquiliza.Shop.Core
{
    public class DefaultDateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
