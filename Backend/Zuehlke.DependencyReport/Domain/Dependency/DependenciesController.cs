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
    public class DependenciesController : Controller
    {
        private DependencyReportContext _context;
        public DependenciesController(DependencyReportContext context)
        {
            _context = context;
        }

        [SwaggerOperation(nameof(GetDependencies))]
        [SwaggerResponse((int) HttpStatusCode.OK, typeof(IEnumerable<DependencyDto>))]
        [HttpGet]
        public IActionResult GetDependencies()
        {
            var dependencies = _context.Dependencies.ToList();
            return Ok(Mapper.Map<IEnumerable<DependencyDto>>(dependencies));
        }

        [SwaggerOperation(nameof(GetDependencyById))]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(DependencyDto))]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [HttpGet("{id}")]
        public IActionResult GetDependencyById(int id)
        {
            var dependency = _context.Dependencies.Where(x => x.Id == id).FirstOrDefault();

            if (dependency != null)
            {
                return Ok(Mapper.Map<DependencyDto>(dependency));
            }
            return NotFound();
        }

        [SwaggerOperation(nameof(CreateDependency))]
        [SwaggerResponse((int)HttpStatusCode.Created, typeof(DependencyDto))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [HttpPost]
        public IActionResult CreateDependency([FromBody] DependencyDto dependencyDto)
        {
            try
            {
                var dependency = Mapper.Map<Dependency>(dependencyDto);
                _context.Dependencies.Add(dependency);
                _context.SaveChanges();
                return Created("api/Dependencies/" + dependency.Id, Mapper.Map<ApplicationDto>(dependency));
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [SwaggerOperation(nameof(UpdateApplication))]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(DependencyDto))]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [HttpPut("{id}")]
        public IActionResult UpdateApplication(int id, [FromBody] DependencyDto dependencyDto)
        {
            var dependency = _context.Dependencies.Where(x => x.Id == id).FirstOrDefault();

            if (dependency != null)
            {
                DependencyHandler.UpdateDependency(dependency, dependencyDto);
                _context.SaveChanges();

                return Ok(Mapper.Map<DependencyDto>(dependency));
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
    }
}
