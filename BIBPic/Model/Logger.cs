using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Serilog.Events;

namespace BIBPic.Model
{
    internal class Logger
    {
                        private static ILogger _logger;
        public static void InitializeLogger()
        {
            _logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }
        public static void Log(string message, LogEventLevel level)
        {
            _logger.Write(level, message);
        }
    

    }
}
