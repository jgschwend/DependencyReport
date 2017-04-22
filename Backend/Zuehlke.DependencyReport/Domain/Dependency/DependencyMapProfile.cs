using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace Zuehlke.DependencyReport
{
    public class DependencyMapProfile : Profile
    {
        public DependencyMapProfile()
        {
            CreateMap<Dependency, DependencyDto>();
            CreateMap<DependencyDto, Dependency>()
                .ForMember(d => d.Id, opt => opt.Ignore());
        }
    }
}
