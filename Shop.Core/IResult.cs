using System;
using System.Collections.Generic;
using System.Text;

namespace Tranquiliza.Shop.Core
{
    public interface IResult
    {
        ResultState State { get; }
        string FailureReason { get; }
    }

    public interface IResult<T> : IResult
    {
        T Data { get; }
    }
}
