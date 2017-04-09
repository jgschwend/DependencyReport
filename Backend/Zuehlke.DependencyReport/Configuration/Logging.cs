using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Zuehlke.DependencyReport.Configuration
{
    public class Logging
    {
        public string LogFolder { get; set; }

        public LogLevel DebugLogLevel { get; set; }
    }
}
