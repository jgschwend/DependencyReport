using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zuehlke.DependencyReport
{
    public class Report
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public DateTime ReportDate { get; set; }

        public string BuildNumber { get; set; }

        public Component Component { get; set; }

        public ICollection<Dependency> Dependencies { get; set; }
    }
}
