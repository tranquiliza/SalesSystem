using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tranquiliza.Shop.Core;
using Tranquiliza.Shop.Core.Application;

namespace Tranquiliza.Shop.Email
{
    public class DefaultMessageSender : IMessageSender, IDisposable
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _smtpEndpoint;
        private readonly string _smtpAccount;
        private readonly string _smtpPassword;

        public DefaultMessageSender(IConfigurationProvider configurationProvider)
        {
            _smtpClient = new SmtpClient();
            _smtpEndpoint = configurationProvider.SmtpEndpointAddress;
            _smtpAccount = configurationProvider.SmtpAccountName;
            _smtpPassword = configurationProvider.SmtpPassword;
        }

        public void Dispose()
        {
            _smtpClient.Disconnect(true);
        }

        // TODO Consider passing username as well (More personalised emails);
        // TODO Fix Sender Address and Names
        public async Task SendMessage(string receiver, string subject, string message)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress("Tranquiliza Webshop", "Tranquiliza-WebShop@gmail.com"));
            mimeMessage.To.Add(new MailboxAddress(receiver));
            mimeMessage.Subject = subject;
            mimeMessage.Body = new TextPart(MimeKit.Text.TextFormat.Plain) {
                Text = message
            };

            await EnsureClientIsConnected().ConfigureAwait(false);
            await _smtpClient.SendAsync(mimeMessage).ConfigureAwait(false);
        }

        private async Task EnsureClientIsConnected()
        {
            if (!_smtpClient.IsConnected)
            {
                await _smtpClient.ConnectAsync(_smtpEndpoint, 465, true).ConfigureAwait(false);
                await _smtpClient.AuthenticateAsync(_smtpAccount, _smtpPassword).ConfigureAwait(false);
            }
        }
    }
}
