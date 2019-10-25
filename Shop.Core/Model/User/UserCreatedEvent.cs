using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tranquiliza.Shop.Core.Model
{
    public class UserCreatedEvent : INotification
    {
        public Guid EmailConfirmationToken { get; }
        public string Email { get; }

        public UserCreatedEvent(Guid emailConfirmationToken, string email)
        {
            EmailConfirmationToken = emailConfirmationToken;
            Email = email;
        }
    }
}
