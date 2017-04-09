using AutoMapper;
using Xunit;

namespace Zuehlke.DependencyReport.Tests.UnitTests
{
    public class ProfileTests
    {
        [Fact]
        public void GivenApplicationMappingProfile_WhenValidateMappings_ThenMappingsAreValid()
        {
            Mapper.Initialize(conf => { conf.AddProfile<ApplicationMapProfile>(); });
            Mapper.AssertConfigurationIsValid();
        }

        [Fact]
        public void GivenComponentMappingProfile_WhenValidateMappings_ThenMappingsAreValid()
        {
            Mapper.Initialize(conf =>
            {
                conf.AddProfile<ComponentMapProfile>();
                conf.AddProfile<ApplicationMapProfile>();
            });
            Mapper.AssertConfigurationIsValid();
        }
    }
}
