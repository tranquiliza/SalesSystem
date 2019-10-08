using System;
using System.Collections.Generic;
using System.Text;

namespace Tranquiliza.Shop.Core
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }
    }
}
