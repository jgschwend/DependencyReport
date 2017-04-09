using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Zuehlke.DependencyReport;

namespace Zuehlke.DependencyReport.DataAccess
{
    public class DependencyReportContext : DbContext
    {
        public DependencyReportContext(DbContextOptions<DependencyReportContext> options): base(options) { }

        public DbSet<Application> Applications { get; set; }
        public DbSet<Component> Components { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Dependency> Dependencies { get; set; }
    }
}
