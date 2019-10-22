using System;
using System.Collections.Generic;
using System.Text;
using Tranquiliza.Shop.Core.Model;

namespace Tranquiliza.Shop.Core.Application
{
    public interface ICreateUserResult
    {
        bool Success { get; }
        string FailureReason { get; }
        User User { get; }
    }
}
