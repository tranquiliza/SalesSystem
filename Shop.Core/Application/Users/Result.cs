using System;
using System.Collections.Generic;
using System.Text;

namespace Tranquiliza.Shop.Core.Application
{
    public class Result : IResult
    {
        public bool Success { get; private set; }

        public string FailureReason { get; private set; }

        public static Result Succeeded => new Result
        {
            FailureReason = string.Empty,
            Success = true
        };

        public static Result Failure(string failureReason) => new Result
        {
            Success = false,
            FailureReason = failureReason
        };
    }
}
