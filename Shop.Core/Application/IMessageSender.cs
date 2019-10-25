using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tranquiliza.Shop.Core.Application
{
    public interface IMessageSender
    {
        public Task SendMessage(string receiver, string subject, string message);
    }
}
