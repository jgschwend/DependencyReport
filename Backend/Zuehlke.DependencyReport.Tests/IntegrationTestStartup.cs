using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Zuehlke.DependencyReport.DataAccess;

namespace Zuehlke.DependencyReport.Tests
{
    public class IntegrationTestStartup : Startup
    {
        public IntegrationTestStartup(IHostingEnvironment env) : base(env)
        {
            env.EnvironmentName = "Testing";
        }

        protected override void SetUpDataBase(IServiceCollection services)
        {
            services
                .AddEntityFramework()
                .AddDbContext<DependencyReportContext>(options =>
                    options.UseInMemoryDatabase()
                );
        }

        protected override void ConfigureDataBase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<DependencyReportContext>();
                dbContext.Database.EnsureCreated();
            }
        }

        protected override void ConfigurareSwaggerGen(SwaggerGenOptions options)
        {
            options.SwaggerDoc("DependencyReport", new Info()
            {
                Title = "Dependency Report API",
                Version = "1.0",
                Description = "Prototype for Dependency Report API",
                Contact = new Contact()
                {
                    Name = "Jonas Gschwend",
                    Email = "jonas.gschwend@zuehlke.com",
                    Url = "http://insight.zuehlke.com/groups/details/7199"
                }
            });
        }
    }
    public class TestRun
    {
        public string DbName { get; set; }
    }
}
