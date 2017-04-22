using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zuehlke.DependencyReport
{
    public class Dependency
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public Component Component { get; set; }

        public string PackageName { get; set; }

        public DependencySource Source { get; set; }

        public DependencyType Type { get; set; }
        
        public string CurrentVersion { get; set; }

        public DateTime CurrentReleaseDate { get; set; }

        public string LatestVersion { get; set; }

        public DateTime LatestReleaseDate { get; set; }

        /*[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public TimeSpan RelativeAge
        {
            get { return LatestReleaseDate.TimeOfDay - CurrentReleaseDate.TimeOfDay; }
            private set { }
        }*/
    }
}
