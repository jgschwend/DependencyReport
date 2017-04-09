using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Zuehlke.DependencyReport.Configuration;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Zuehlke.DependencyReport.DataAccess;

namespace Zuehlke.DependencyReport
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        internal Swagger SwaggerConfig { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            
            SwaggerConfig = Configuration.GetSection(nameof(Swagger)).Get<Swagger>();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            services.AddSwaggerGen(ConfigurareSwaggerGen);
            SetUpDataBase(services);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                loggerFactory.AddConsole();
                loggerFactory.AddDebug();
            }

            app.UseMvc();

            app.UseSwagger(c =>
            {                
                c.RouteTemplate = "api/docs/{documentName}/swagger.json";
                c.PreSerializeFilters.Add((swagger, httpReq) => swagger.Host = SwaggerConfig.Host);
            });

            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "api/docs";
                c.SwaggerEndpoint("/api/docs/DependencyReport/swagger.json", "Dependency Report API");
            });

            Mapper.Initialize(config =>
            {
                config.AddProfile<ApplicationMapProfile>();
                config.AddProfile<ComponentMapProfile>();
                config.AddProfile<ReportMapProfile>();
                config.AddProfile<DependencyMapProfile>();
            });
            Mapper.AssertConfigurationIsValid();

            ConfigureDataBase(app);

            if (env.IsDevelopment())
            {
                var data = System.IO.File.ReadAllText(@"DataAccess\applicationdataseed.json");
                Seed.Seedit(data, app.ApplicationServices);
            }
        }

        protected virtual void ConfigurareSwaggerGen(SwaggerGenOptions options)
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

            var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, SwaggerConfig.FileName);
            options.IncludeXmlComments(filePath);
            options.DescribeAllEnumsAsStrings();
        }

        protected virtual void SetUpDataBase(IServiceCollection services)
        {
            services
                .AddEntityFramework()
                .AddEntityFrameworkSqlServer()
                .AddDbContext<DependencyReportContext>(options =>
                    options.UseSqlServer(
                        Configuration.GetConnectionString("DependencyReportConnectionStrings")
                    )
                );
        }

        protected virtual void ConfigureDataBase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<DependencyReportContext>();
                context.Database.Migrate();
            }
        }
    }
}
