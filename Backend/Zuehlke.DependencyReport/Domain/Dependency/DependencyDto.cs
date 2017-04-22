using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zuehlke.DependencyReport
{
    public class DependencyDto
    {
        public long Id { get; set; }

        public ComponentDto Component { get; set; }

        public string PackageName { get; set; }

        public DependencySource Source { get; set; }

        public DependencyType Type { get; set; }
        
        public string CurrentVersion { get; set; }

        public DateTime CurrentReleaseDate { get; set; }

        public string LatestVersion { get; set; }

        public DateTime LatestReleaseDate { get; set; }

        /*public TimeSpan RelativeAge { get; set; }*/
    }
}
