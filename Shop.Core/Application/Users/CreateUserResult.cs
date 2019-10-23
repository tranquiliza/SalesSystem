using System;
using System.Collections.Generic;
using System.Text;
using Tranquiliza.Shop.Core.Model;

namespace Tranquiliza.Shop.Core.Application
{
    public class CreateUserResult : ICreateUserResult
    {
        public bool Success { get; private set; }

        public string FailureReason { get; private set; }

        public User User { get; private set; }

        public static CreateUserResult Succeeded(User user) => new CreateUserResult
        {
            User = user,
            FailureReason = string.Empty,
            Success = true
        };

        public static CreateUserResult Failure(string failureReason) => new CreateUserResult
        {
            Success = false,
            FailureReason = failureReason
        };
    }
}
