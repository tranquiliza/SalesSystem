using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tranquiliza.Shop.Core.Model
{
    public class UserEmailConfirmedEvent : INotification
    {
        public string Email { get; }

        public UserEmailConfirmedEvent(string email)
        {
            Email = email;
        }
    }
}
