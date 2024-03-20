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
        private ILogger _logger;

        public ILogger GetLogger()
        {
            if (_logger == null)
            {
                _logger = new LoggerConfiguration().CreateLogger();
            }
            return _logger;
        }

       
    

    }
}
