using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Zuehlke.DependencyReport.DataAccess;


namespace Zuehlke.DependencyReport
{
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "DependencyReport")]
    public class ReportsController : Controller
    {
        private DependencyReportContext _context;
        public ReportsController(DependencyReportContext context)
        {
            _context = context;
        }

        [SwaggerOperation(nameof(GetReports))]
        [SwaggerResponse((int) HttpStatusCode.OK, typeof(IEnumerable<ReportDto>))]
        [HttpGet]
        public IActionResult GetReports()
        {
            var reports = _context.Reports.ToList();
            return Ok(Mapper.Map<IEnumerable<ReportDto>>(reports));
        }

        [SwaggerOperation(nameof(GetReportById))]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(ReportDto))]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [HttpGet("{id}")]
        public IActionResult GetReportById(int id)
        {
            var report = _context.Reports.Where(x => x.Id == id).FirstOrDefault();

            if (report != null)
            {
                return Ok(Mapper.Map<ReportDto>(report));
            }
            return NotFound();
        }

        [SwaggerOperation(nameof(CreateReport))]
        [SwaggerResponse((int)HttpStatusCode.Created, typeof(ReportDto))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [HttpPost]
        public IActionResult CreateReport([FromBody] ReportDto reportDto)
        {
            try
            {
                var report = Mapper.Map<Report>(reportDto);
                _context.Reports.Add(report);
                _context.SaveChanges();
                return Created("api/Reports/" + report.Id, Mapper.Map<ReportDto>(report));
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [SwaggerOperation(nameof(UpdateReport))]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(ReportDto))]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [HttpPut("{id}")]
        public IActionResult UpdateReport(int id, [FromBody] ReportDto reportDto)
        {
            var report = _context.Reports.Where(x => x.Id == id).FirstOrDefault();
            ReportHandler.UpdateReport(report,reportDto);

            _context.SaveChanges();

            return Ok(Mapper.Map<ReportDto>(report));
        }

        [SwaggerOperation(nameof(DeleteReport))]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [HttpDelete("{id}")]
        public IActionResult DeleteReport(int id)
        {
            var report = _context.Reports.Where(x => x.Id == id).FirstOrDefault();

            if (report != null)
            {
                _context.Reports.Remove(report);
                _context.SaveChanges();
                return Ok();
            }
            return NotFound();
        }

        [SwaggerOperation(nameof(GetDependenciesByReportId))]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(IEnumerable<DependencyDto>))]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [HttpGet("{id}/Dependencies")]
        public IActionResult GetDependenciesByReportId(int id)
        {
            var dependencies = _context.Dependencies.Where(d => d.Component.Id == id);

            return Ok(Mapper.Map<IEnumerable<DependencyDto>>(dependencies));
        }
    }
}
