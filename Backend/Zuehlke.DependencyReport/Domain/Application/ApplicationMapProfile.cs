using AutoMapper;

namespace Zuehlke.DependencyReport
{
    public class ApplicationMapProfile : Profile
    {
        public ApplicationMapProfile()
        {
            CreateMap<Application, ApplicationDto>();
            CreateMap<ApplicationDto, Application>()
                .ForMember(d => d.Id, opt => opt.Ignore());

        }
    }
}