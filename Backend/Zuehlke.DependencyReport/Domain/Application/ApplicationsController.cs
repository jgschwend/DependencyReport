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
    public class ApplicationsController : Controller
    {
        private DependencyReportContext _context;
        public ApplicationsController(DependencyReportContext context)
        {
            _context = context;
        }

        [SwaggerOperation(nameof(GetApplications))]
        [SwaggerResponse((int) HttpStatusCode.OK, typeof(IEnumerable<ApplicationDto>))]
        [HttpGet]
        public IActionResult GetApplications()
        {
            var applications = _context.Applications.ToList();
            return Ok(Mapper.Map<IEnumerable<ApplicationDto>>(applications));
        }

        [SwaggerOperation(nameof(GetApplicationById))]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(ApplicationDto))]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [HttpGet("{id}")]
        public IActionResult GetApplicationById(int id)
        {
            var application = _context.Applications.Where(x => x.Id == id).FirstOrDefault();

            if (application != null)
            {
                return Ok(Mapper.Map<ApplicationDto>(application));
            }
            return NotFound();
        }

        [SwaggerOperation(nameof(CreateApplication))]
        [SwaggerResponse((int)HttpStatusCode.Created, typeof(ApplicationDto))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [HttpPost]
        public IActionResult CreateApplication([FromBody] ApplicationDto applicationDto)
        {
            try
            {
                var application = Mapper.Map<Application>(applicationDto);
                _context.Applications.Add(application);
                _context.SaveChanges();
                return Created("api/Applications/" + application.Id, Mapper.Map<ApplicationDto>(application));
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [SwaggerOperation(nameof(UpdateApplication))]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(ApplicationDto))]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [HttpPut("{id}")]
        public IActionResult UpdateApplication(int id, [FromBody] ApplicationDto applicationDto)
        {
            var application = _context.Applications.Where(x => x.Id == id).FirstOrDefault();
            if (application != null)
            {
                application.Name = applicationDto.Name;
                application.Description = applicationDto.Description;
                _context.SaveChanges();

                return Ok(Mapper.Map<ApplicationDto>(application));
            }
            return NotFound();
        }

        [SwaggerOperation(nameof(DeleteApplication))]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [HttpDelete("{id}")]
        public IActionResult DeleteApplication(int id)
        {
            var application = _context.Applications.Where(x => x.Id == id).FirstOrDefault();

            if (application != null)
            {
                _context.Applications.Remove(application);
                _context.SaveChanges();
                return Ok();
            }
            return NotFound();
        }

        [SwaggerOperation(nameof(GetComponentsByApplicationId))]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(IEnumerable<ComponentDto>))]
        [HttpGet("{id}/Components")]
        public IActionResult GetComponentsByApplicationId(int id)
        {
            var components = _context.Components.Where(x => x.Application.Id == id);

            return Ok(Mapper.Map<IEnumerable<ComponentDto>>(components));
        }

        [SwaggerOperation(nameof(GetReportsByApplicationId))]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(IEnumerable<ReportDto>))]
        [HttpGet("{id}/Reports")]
        public IActionResult GetReportsByApplicationId(int id)
        {
            var reports = _context.Reports.Where(x => x.Component.Application.Id == id);

            return Ok(Mapper.Map<IEnumerable<ReportDto>>(reports));
        }

        [SwaggerOperation(nameof(GetDependenciesByApplicationId))]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(IEnumerable<DependencyDto>))]
        [HttpGet("{id}/Dependencies")]
        public IActionResult GetDependenciesByApplicationId(int id)
        {
            var dependencies = _context.Reports.Where(x => x.Component.Application.Id == id).Select(x => x.Dependencies);

            return Ok(Mapper.Map<IEnumerable<DependencyDto>>(dependencies));
        }
    }
}
