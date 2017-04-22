using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace Zuehlke.DependencyReport
{
    public class ReportMapProfile : Profile
    {
        public ReportMapProfile()
        {
            CreateMap<Report, ReportDto>();
            CreateMap<ReportDto, Report>()
                .ForMember(d => d.Id, opt => opt.Ignore());
        }
    }
}
