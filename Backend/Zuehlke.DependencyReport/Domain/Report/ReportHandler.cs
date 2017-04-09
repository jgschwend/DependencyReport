using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zuehlke.DependencyReport
{
    public class ReportHandler
    {
        public static void UpdateReport(Report report, ReportDto reportDto)
        {
            report.BuildNumber = reportDto.BuildNumber;
            report.ReportDate = reportDto.ReportDate;
        }
    }
}
