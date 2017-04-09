using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zuehlke.DependencyReport
{
    public class DependencyHandler
    {
        public static void UpdateDependency(Dependency dependency, DependencyDto dependencyDto)
        {
            dependency.PackageName = dependencyDto.PackageName;
            dependency.CurrentVersion = dependencyDto.CurrentVersion;
            dependency.CurrentReleaseDate = dependencyDto.CurrentReleaseDate;
            dependency.LatestVersion = dependencyDto.LatestVersion;
            dependency.LatestReleaseDate = dependencyDto.LatestReleaseDate;
            dependency.Source = dependencyDto.Source;
            dependency.Type = dependencyDto.Type;
        }
    }
}
