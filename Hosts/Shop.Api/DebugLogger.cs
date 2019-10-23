using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tranquiliza.Shop.Core;

namespace Tranquiliza.Shop.Api
{
    public class DebugLogger : ILogger
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public DebugLogger(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        private const string DateTimeFormat = "yyyy'-'MM'-'dd hh':'mm':'ss";

        private void WriteToConsole(string message, Exception ex = null)
        {
            Debug($"{_dateTimeProvider.UtcNow.ToString(DateTimeFormat)} - {message}");

            if (ex != null)
                Debug($"{_dateTimeProvider.UtcNow.ToString(DateTimeFormat)} - {ex}");
        }

        public void Debug(string message)
        {
            WriteToConsole(message);
        }

        public void Fatal(string message, Exception ex = null)
        {
            WriteToConsole(message, ex);
        }

        public void Info(string message)
        {
            WriteToConsole(message);
        }

        public void Warning(string message, Exception ex = null)
        {
            WriteToConsole(message, ex);
        }
    }
}
