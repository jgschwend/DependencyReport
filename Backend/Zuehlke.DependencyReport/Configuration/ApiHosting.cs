using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zuehlke.DependencyReport.Configuration
{
    public class ApiHosting
    {
        public SupportedPortocol Protocol { get; set; }

        public int Port { get; set; }

        public string SSLCertThumbprint { get; set; }
    }

    public enum SupportedPortocol
    {
        HTTP = 0, HTTPS
    }
}
