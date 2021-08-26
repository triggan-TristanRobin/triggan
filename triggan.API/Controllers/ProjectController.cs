using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using triggan.BlogManager;
using triggan.BlogManager.Interfaces;
using triggan.BlogModel;

namespace triggan.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly BlogAccessor accessor;

        public ProjectController(BlogAccessor accessor)
        {
            this.accessor = accessor;
        }

        [HttpGet]
        public IEnumerable<Project> Get([FromQuery] int count = 0)
        {
            return accessor.GetAll<Project>(count);
        }

        [HttpGet("{slug}")]
        public Project Get(string slug)
        {
            var project = accessor.Get(slug) as Project;
            return project;
        }

        [HttpGet("{slug}/Updates")]
        public List<Update> GetUpdates(string slug)
        {
            return accessor.GetProjectUpdates(slug);
        }

        [HttpPost()]
        public Project Post(Project project)
        {
            return accessor.Add(project);
        }

        [HttpPost("{slug}/Updates")]
        public Project Update(string slug, Update update)
        {
            return accessor.AddUpdate(slug, update);
        }

        [HttpPut("{slug}")]
        public Project Put(string slug, Project project)
        {
            return accessor.Update(slug, project);
        }

        [HttpPut("{slug}/Updates")]
        public Project Update(string slug, List<Update> updates)
        {
            return accessor.SetUpdates(slug, updates);
        }

        [HttpDelete("{slug}")]
        public Project Delete(string slug)
        {
            return accessor.Delete(slug) as Project;
        }
    }
}