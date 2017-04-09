using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

namespace Zuehlke.DependencyReport.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            if (args.IsArgumentSet("Path") && args.IsArgumentSet("ApplicationId"))
            {
                var path = args.GetArgumentValue("Path");
                var applicationId = args.GetArgumentValue("ApplicationId");

                if (Directory.Exists(path))
                {
                    var nugetFiles = Directory.GetFiles(path, "packages.config", SearchOption.AllDirectories);
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
                                var componentName = Directory.GetParent(nugetFile).Name;

                                var compResponse = client.GetAsync("api/Applications/" + applicationId + "/Components/" + componentName).Result;
                                if (compResponse.IsSuccessStatusCode)
                                {
                                    var compResult = compResponse.Content.ReadAsStringAsync().Result;
                                    var component = JsonConvert.DeserializeObject<ComponentDto>(compResult);
                                }
                                else
                                {
                                    ComponentDto componentDto = new ComponentDto()
                                    {
                                        Name = componentName
                                    };

                                    var content = new StringContent(JsonConvert.SerializeObject(componentDto), Encoding.UTF8, "application/json");
                                    var postResult = client.PostAsync("api/Applications/" + applicationId + "/Components", content).Result;

                                    var bla = postResult.Content.ReadAsStringAsync().Result;
                                }
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
    }
}