using System;
using System.Collections.Generic;
using System.Text;
using Tranquiliza.Shop.Core.Model;

namespace Tranquiliza.Shop.Core
{
    public interface IApplicationContext
    {
        Guid UserId { get; }
        Guid ClientId { get; }
        bool IsAnonymous { get; }
    }
}
