using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zuehlke.DependencyReport.Configuration
{
    public class Swagger
    {
        /// <summary>
        /// Filename of the xml comments to have better swagger doc
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// used to determine host name e.g. external dns name for loadbalancing on higher stages
        /// </summary>
        public string Host { get; set; }
    }
}
