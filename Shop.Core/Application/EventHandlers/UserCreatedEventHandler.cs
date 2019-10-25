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

        public UserCreatedEventHandler(IMessageSender messageSender)
        {
            _messageSender = messageSender;
        }

        public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
        {
            var title = "Confirm your email!";
            var message = $"Confirm your email by pressing this link: https://localhost:44311/confirm?{notification.EmailConfirmationToken}";

            await _messageSender.SendMessage(notification.Email, title, message).ConfigureAwait(false);

            System.Diagnostics.Debug.WriteLine($"Handled UserCreatedEvent with email: {notification.Email} and token {notification.EmailConfirmationToken}");
        }
    }
}
