using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Zuehlke.DependencyReport.Configuration;

namespace Zuehlke.DependencyReport
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {

                var config = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("hosting.json", false)
                        .AddJsonFile("hosting.Development.json", true)
                        .Build();

                var apiConfiguration = config.GetSection(nameof(ApiHosting)).Get<ApiHosting>();

                var webHostBuilder = new WebHostBuilder()
                    .UseKestrel()
                    .UseUrls($"{apiConfiguration.Protocol}://+:{apiConfiguration.Port}")
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseIISIntegration()
                    .UseStartup<Startup>()
                    .UseApplicationInsights()
                    .Build();

                webHostBuilder.Run();
            }
            catch (Exception exception)
            {
               var a = exception.InnerException;
            }
        }
    }
}
