using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using triggan.BlogManager.Interfaces;
using triggan.BlogModel;

namespace triggan.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly ISlugRepository<Project> repository;

        public ProjectController(ISlugRepository<Project> repo)
        {
            this.repository = repo;
        }

        [HttpGet]
        public IEnumerable<Project> Get([FromQuery] int count = 0)
        {
            return repository.Get(count: count, includeProperties: "Updates");
        }

        [HttpGet("{slug}")]
        public Project Get(string slug)
        {
            return repository.Get(slug);
        }
    }
}