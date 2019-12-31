using Serilog;
using System;
using Tranquiliza.Shop.Core;

namespace Tranquiliza.Shop.Api
{
    public partial class Startup
    {
        public class LogBridge : IApplicationLogger
        {
            private readonly ILogger _logger;

            public LogBridge(ILogger logger)
            {
                _logger = logger;
            }

            public void Debug(string message)
            {
                _logger.Debug("Debug: {Message}", message);
            }

            public void Fatal(string message, Exception ex = null)
            {
                if (ex != null)
                    _logger.Fatal(ex, "Fatal: {Message}", message);
                else
                    _logger.Fatal("Fatal: {Message}", message);
            }

            public void Info(string message)
            {
                _logger.Information("Info: {Message}", message);
            }

            public void Warning(string message, Exception ex = null)
            {
                if (ex != null)
                    _logger.Warning(ex, "Warning: {Message}", message);
                else
                    _logger.Warning("Warning: {Message}", message);
            }
        }
    }
}
