using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using Xunit;

namespace Zuehlke.DependencyReport.Tests.IntegrationTests
{
    public abstract class IntegrationTestBase : IClassFixture<ApiFixture>
    {
        public readonly TestServer Server;
        public readonly HttpClient Client;

        protected IntegrationTestBase(ApiFixture apiFixture)
        {
            Server = apiFixture.Server;
            Client = apiFixture.Client;
        }
    }
}
