using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zuehlke.DependencyReport
{
    public class ReportDto
    {
        public long Id { get; set; }

        public DateTime ReportDate { get; set; }

        public string BuildNumber { get; set; }

        public ComponentDto Component { get; set; }

        public ICollection<DependencyDto> Dependencies { get; set; }
    }
}
