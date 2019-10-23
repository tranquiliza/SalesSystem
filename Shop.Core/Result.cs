using System;
using System.Collections.Generic;
using System.Text;

namespace Tranquiliza.Shop.Core
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

    public class Result<T> : IResult<T>
    {
        public T Data { get; private set; }

        public bool Success { get; private set; }

        public string FailureReason { get; private set; }

        public static Result<T> Succeeded(T data) => new Result<T>
        {
            Success = true,
            Data = data
        };

        public static Result<T> Failure(string failure) => new Result<T>
        {
            Success = false,
            FailureReason = failure
        };
    }
}
