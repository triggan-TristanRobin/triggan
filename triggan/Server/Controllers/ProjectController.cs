using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.Enums;
using triggan.Interfaces;

namespace triggan.Server.Controllers
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
        public IEnumerable<Project> Get()
        {
            return repository.Get();
        }

        [HttpGet("{slug}")]
        public Project Get(string slug)
        {
            return repository.Get(slug);
        }
    }
}
