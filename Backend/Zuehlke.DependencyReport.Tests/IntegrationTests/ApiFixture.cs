using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Net.Http;

namespace Zuehlke.DependencyReport.Tests.IntegrationTests
{
    public class ApiFixture : IDisposable
    {
        public TestServer Server { get; }
        public HttpClient Client { get; }

        public ApiFixture()
        {
            var builder = new WebHostBuilder()
              .UseEnvironment("Development")
              .UseStartup<IntegrationTestStartup>();

            Server = new TestServer(builder);
            Client = Server.CreateClient();
        }

        public void Dispose()
        {
            Server.Dispose();
            Client.Dispose();
        }
    }
}
