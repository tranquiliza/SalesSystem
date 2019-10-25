using MediatR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tranquiliza.Shop.Core.Model;

namespace Tranquiliza.Shop.Core.Application.EventHandlers
{
    public class UserCreatedEventHandler : INotificationHandler<UserCreatedEvent>
    {
        private readonly IMessageSender _messageSender;
        private readonly string _hostName;

        public UserCreatedEventHandler(IMessageSender messageSender, IConfigurationProvider configurationProvider)
        {
            _messageSender = messageSender;
            _hostName = configurationProvider.HostName;
        }

        public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
        {
            // TODO Make configurable email formatting for system.
            var title = "Confirm your email!";
            var message = $"Confirm your email by pressing this link: {_hostName}users/confirm?userId={notification.UserId}&emailConfirmationToken={notification.EmailConfirmationToken}";

            await _messageSender.SendMessage(notification.Email, title, message).ConfigureAwait(false);
        }
    }
}
