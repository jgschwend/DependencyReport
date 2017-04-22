using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Zuehlke.DependencyReport.Console.PackageManagement;

namespace Zuehlke.DependencyReport.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            if (args.IsArgumentSet("Path") && args.IsArgumentSet("ApplicationId") && args.IsArgumentSet("BuildNumber"))
            {
                var path = args.GetArgumentValue("Path");
                var applicationId = args.GetArgumentValue("ApplicationId");
                var buildNumber = args.GetArgumentValue("BuildNumber");

                if (Directory.Exists(path))
                {

                    var nugetFiles = GetNugetListByComponent(path);
                    var bowerFiles = Directory.GetFiles(path, "bower.json", SearchOption.AllDirectories);
                    var npmFiles = Directory.GetFiles(path, "package.json", SearchOption.AllDirectories);

                    using (var client = new HttpClient() {BaseAddress = new Uri("http://localhost:45736")})
                    {
                        var appResponse = client.GetAsync("api/Applications/" + applicationId).Result;

                        if (appResponse.IsSuccessStatusCode)
                        {
                            var appResult = appResponse.Content.ReadAsStringAsync().Result;
                            var application = JsonConvert.DeserializeObject<ApplicationDto>(appResult);

                            foreach (var nugetFile in nugetFiles)
                            {
                                ComponentDto component = null;

                                var getResponse = client.GetAsync("api/Applications/" + applicationId + "/Components/" + nugetFile.Key).Result;
                                if (getResponse.IsSuccessStatusCode)
                                {
                                    var getResult = getResponse.Content.ReadAsStringAsync().Result;
                                    component = JsonConvert.DeserializeObject<ComponentDto>(getResult);
                                }
                                else
                                {
                                    ComponentDto componentDto = new ComponentDto()
                                    {
                                        Name = nugetFile.Key
                                    };

                                    var content = new StringContent(JsonConvert.SerializeObject(componentDto), Encoding.UTF8, "application/json");
                                    var postResponse = client.PostAsync("api/Applications/" + applicationId + "/Components", content).Result;

                                    var postResult = postResponse.Content.ReadAsStringAsync().Result;
                                    component = JsonConvert.DeserializeObject<ComponentDto>(postResult);
                                }

                                ReportDto reportDto = new ReportDto()
                                {
                                    BuildNumber = buildNumber,
                                    Component = component,
                                    ReportDate = new DateTime(),
                                    Dependencies = new List<DependencyDto>()
                                };

                                foreach (var package in nugetFile.Value.Nuget)
                                {
                                    DependencyDto dependency = new DependencyDto()
                                    {
                                        PackageName = package.id,
                                        CurrentVersion = package.version,
                                        Source = DependencySource.NuGet
                                    };

                                    reportDto.Dependencies.Add(dependency);
                                }

                                var reportContent = new StringContent(JsonConvert.SerializeObject(reportDto), Encoding.UTF8, "application/json");
                                var reportResponse = client.PostAsync("api/Reports", reportContent).Result;

                            }
                        }
                        else
                        {
                            System.Console.WriteLine("ApplicationId is invalid");
                        }
                    }
                }
                else
                {
                    System.Console.WriteLine("Path '" + path + "' is not valid");
                }
            }
            else
            {
                if (!args.IsArgumentSet("Path"))
                {
                    System.Console.WriteLine("Missing parameter 'Path'");
                }
                if (!args.IsArgumentSet("ApplicationId"))
                {
                    System.Console.WriteLine("Missing parameter 'ApplicationId'");
                }
            }

            watch.Stop();

            System.Console.WriteLine("I'm done after {0} seconds", watch.Elapsed.TotalSeconds);
            System.Console.ReadLine();
        }

        public static List<KeyValuePair<string, NugetPackageConfig>> GetNugetListByComponent(string path)
        {
            List<KeyValuePair<string, NugetPackageConfig>> nugetKeyValuePairs = new List<KeyValuePair<string, NugetPackageConfig>>();

            var nugetFiles = Directory.GetFiles(path, "packages.config", SearchOption.AllDirectories);

            foreach (var nuget in nugetFiles)
            {
                var parentDirectory = Directory.GetParent(nuget).FullName;
                var projectFilePath = Directory.GetFiles(parentDirectory, "*.csproj", SearchOption.TopDirectoryOnly)[0];
                var projectFileName = Path.GetFileNameWithoutExtension(projectFilePath);
                NugetPackageConfig packages = (NugetPackageConfig)new XmlSerializer(typeof(NugetPackageConfig)).Deserialize(XmlReader.Create(nuget));

                nugetKeyValuePairs.Add(new KeyValuePair<string, NugetPackageConfig>(projectFileName,packages));
            }

            return nugetKeyValuePairs;
        }
    }
}