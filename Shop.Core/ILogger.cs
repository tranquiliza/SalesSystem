using System;
using System.Collections.Generic;
using System.Text;

namespace Tranquiliza.Shop.Core
{
    public interface ILogger
    {
        void Debug(string message);
        void Info(string message);
        void Warning(string message, Exception ex = null);
        void Fatal(string message, Exception ex = null);
    }
}
