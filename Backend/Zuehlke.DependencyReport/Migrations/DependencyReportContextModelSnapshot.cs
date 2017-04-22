using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Zuehlke.DependencyReport.DataAccess;
using Zuehlke.DependencyReport;

namespace Zuehlke.DependencyReport.Migrations
{
    [DbContext(typeof(DependencyReportContext))]
    partial class DependencyReportContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Zuehlke.DependencyReport.Application", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Applications");
                });

            modelBuilder.Entity("Zuehlke.DependencyReport.Component", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("ApplicationId");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId");

                    b.ToTable("Components");
                });

            modelBuilder.Entity("Zuehlke.DependencyReport.Dependency", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("ComponentId");

                    b.Property<DateTime>("CurrentReleaseDate");

                    b.Property<string>("CurrentVersion");

                    b.Property<DateTime>("LatestReleaseDate");

                    b.Property<string>("LatestVersion");

                    b.Property<string>("PackageName");

                    b.Property<long?>("ReportId");

                    b.Property<int>("Source");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("ComponentId");

                    b.HasIndex("ReportId");

                    b.ToTable("Dependencies");
                });

            modelBuilder.Entity("Zuehlke.DependencyReport.Report", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BuildNumber");

                    b.Property<long?>("ComponentId");

                    b.Property<DateTime>("ReportDate");

                    b.HasKey("Id");

                    b.HasIndex("ComponentId");

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("Zuehlke.DependencyReport.Component", b =>
                {
                    b.HasOne("Zuehlke.DependencyReport.Application", "Application")
                        .WithMany()
                        .HasForeignKey("ApplicationId");
                });

            modelBuilder.Entity("Zuehlke.DependencyReport.Dependency", b =>
                {
                    b.HasOne("Zuehlke.DependencyReport.Component", "Component")
                        .WithMany()
                        .HasForeignKey("ComponentId");

                    b.HasOne("Zuehlke.DependencyReport.Report")
                        .WithMany("Dependencies")
                        .HasForeignKey("ReportId");
                });

            modelBuilder.Entity("Zuehlke.DependencyReport.Report", b =>
                {
                    b.HasOne("Zuehlke.DependencyReport.Component", "Component")
                        .WithMany()
                        .HasForeignKey("ComponentId");
                });
        }
    }
}
