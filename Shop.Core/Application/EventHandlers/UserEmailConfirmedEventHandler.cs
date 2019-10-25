using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tranquiliza.Shop.Core.Model;

namespace Tranquiliza.Shop.Core.Application.EventHandlers
{
    public class UserEmailConfirmedEventHandler : INotificationHandler<UserEmailConfirmedEvent>
    {
        private readonly IMessageSender _messageSender;

        public UserEmailConfirmedEventHandler(IMessageSender messageSender)
        {
            _messageSender = messageSender;
        }

        public async Task Handle(UserEmailConfirmedEvent notification, CancellationToken cancellationToken)
        {
            await _messageSender.SendMessage(notification.Email, "Thank you for confirming your email", "Welcome! And thank you for the confirmation! Enjoy!").ConfigureAwait(false);
        }
    }
}
