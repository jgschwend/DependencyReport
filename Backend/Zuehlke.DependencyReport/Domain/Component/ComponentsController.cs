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
    public class ComponentsController : Controller
    {
        private DependencyReportContext _context;
        public ComponentsController(DependencyReportContext context)
        {
            _context = context;
        }

        [SwaggerOperation(nameof(GetComponents))]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(IEnumerable<ComponentDto>))]
        [HttpGet]
        public IActionResult GetComponents()
        {
            var components = _context.Components.ToList();
            return Ok(Mapper.Map<IEnumerable<ComponentDto>>(components));
        }

        [SwaggerOperation(nameof(GetComponentById))]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(ComponentDto))]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [HttpGet("{id}")]
        public IActionResult GetComponentById(int id)
        {
            var component = _context.Components.Where(c => c.Id == id).FirstOrDefault();

            if (component != null)
            {
                return Ok(Mapper.Map<ComponentDto>(component));
            }
            return NotFound();
        }

        [SwaggerOperation(nameof(CreateComponent))]
        [SwaggerResponse((int)HttpStatusCode.Created, typeof(ComponentDto))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [HttpPost]
        public IActionResult CreateComponent([FromBody] ComponentDto componentDto)
        {
            try
            {
                var component = Mapper.Map<Component>(componentDto);
                _context.Components.Add(component);
                _context.SaveChanges();
                return Created("api/Components/" + component.Id, Mapper.Map<ComponentDto>(component));
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [SwaggerOperation(nameof(UpdateComponent))]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(ComponentDto))]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [HttpPut("{id}")]
        public IActionResult UpdateComponent(int id, [FromBody] ComponentDto componentDto)
        {
            var component = _context.Components.Where(x => x.Id == id).FirstOrDefault();

            if (component != null)
            {
                component.Name = componentDto.Name;
                component.Description = componentDto.Description;
                _context.SaveChanges();

                return Ok(Mapper.Map<ComponentDto>(component));
            }
            return NotFound();
        }

        [SwaggerOperation(nameof(DeleteComponent))]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [HttpDelete("{id}")]
        public IActionResult DeleteComponent(int id)
        {
            var component = _context.Components.Where(x => x.Id == id).FirstOrDefault();

            if (component != null)
            {
                _context.Components.Remove(component);
                _context.SaveChanges();
                return Ok();
            }
            return NotFound();
        }

        [SwaggerOperation(nameof(GetReportsByComponentId))]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(IEnumerable<ReportDto>))]
        [HttpGet("{id}/Reports")]
        public IActionResult GetReportsByComponentId(int id)
        {
            var reports = _context.Reports.Where(x => x.Component.Id == id);

            return Ok(Mapper.Map<IEnumerable<ReportDto>>(reports));
        }

        [SwaggerOperation(nameof(GetDependenciesByComponentId))]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(IEnumerable<DependencyDto>))]
        [HttpGet("{id}/Dependencies")]
        public IActionResult GetDependenciesByComponentId(int id)
        {
            var dependencies = _context.Reports.Where(x => x.Component.Application.Id == id).Select(x => x.Dependencies);

            return Ok(Mapper.Map<IEnumerable<DependencyDto>>(dependencies));
        }
    }
}
