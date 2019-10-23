using System;
using System.Collections.Generic;
using System.Text;

namespace Tranquiliza.Shop.Core.Application
{
    public interface IResult
    {
        bool Success { get; }
        string FailureReason { get; }
    }
}
