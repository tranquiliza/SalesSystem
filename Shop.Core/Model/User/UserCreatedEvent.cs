using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tranquiliza.Shop.Core.Model
{
    public class UserCreatedEvent : INotification
    {
        public Guid EmailConfirmationToken { get; }
        public Guid UserId { get; }
        public string Email { get; }

        public UserCreatedEvent(Guid emailConfirmationToken, Guid userId, string email)
        {
            EmailConfirmationToken = emailConfirmationToken;
            UserId = userId;
            Email = email;
        }
    }
}
