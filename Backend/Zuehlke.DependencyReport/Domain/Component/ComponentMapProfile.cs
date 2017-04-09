using AutoMapper;

namespace Zuehlke.DependencyReport
{
    public class ComponentMapProfile : Profile
    {
        public ComponentMapProfile()
        {
            CreateMap<Component, ComponentDto>();
            CreateMap<ComponentDto, Component>();
        }
    }
}
