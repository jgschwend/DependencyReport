using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Zuehlke.DependencyReport.DataAccess
{
    public static class Seed
    {
        public static void Seedit(string jsonData, IServiceProvider serviceProvider)
        {
            List<Application> applications =
                JsonConvert.DeserializeObject<List<Application>>(jsonData);
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<DependencyReportContext>();
                if (!context.Applications.Any())
                {
                    context.AddRange(applications);
                    context.SaveChanges();
                }
            }

        }
    }
}
