using System.Threading.Tasks;
using Xunit;

namespace Zuehlke.DependencyReport.Tests.IntegrationTests
{
    public class ApplicationControllerTests : IntegrationTestBase
    {
        public ApplicationControllerTests(ApiFixture apiFixture) : base(apiFixture) { }

        [Fact]
        public async Task Test()
        {
            var response = await Client.GetAsync("/api/Applications/");
            var result = await response.Content.ReadAsStringAsync();

            Assert.Equal(2, result.Length);
        }
    }
}