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
            System.Diagnostics.Debug.WriteLine($"{_dateTimeProvider.UtcNow.ToString(DateTimeFormat)} - {message}");

            if (ex != null)
                System.Diagnostics.Debug.WriteLine($"{_dateTimeProvider.UtcNow.ToString(DateTimeFormat)} - {ex}");
        }

        public void Debug(string message)
        {
            WriteToConsole("DEBUG:   " + message);
        }

        public void Fatal(string message, Exception ex = null)
        {
            WriteToConsole("FATAL:   " + message, ex);
        }

        public void Info(string message)
        {
            WriteToConsole("INFO:    " + message);
        }

        public void Warning(string message, Exception ex = null)
        {
            WriteToConsole("WARNING: " + message, ex);
        }
    }
}
