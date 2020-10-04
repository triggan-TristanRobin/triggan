using System.Collections.Generic;
using System.Linq;
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

        [HttpGet("[action]")]
        public IEnumerable<Project> GetAll(int count)
        {
            return repository.Get(orderBy: source => source.OrderBy(proj => proj.Updated), count: count);
        }

        [HttpGet("[action]")]
        public Project Get(string slug)
        {
            return repository.Get(slug);
        }
    }
}
