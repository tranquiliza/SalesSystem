using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tranquiliza.Shop.Core.Model;

namespace Tranquiliza.Shop.Core.Application.EventHandlers
{
    public class UserCreatedEventHandler : INotificationHandler<UserCreatedEvent>
    {
        public Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
        {
            // TODO Implement actual logic for sending emails

            System.Diagnostics.Debug.WriteLine($"Handled UserCreatedEvent with email: {notification.Email} and token {notification.EmailConfirmationToken}");

            return Task.CompletedTask;
        }
    }
}
