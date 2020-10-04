using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.Enums;
using triggan.Interfaces;

namespace triggan.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private readonly ISlugRepository<Post> repository;

        public PostController(ISlugRepository<Post> repo)
        {
            this.repository = repo;
        }

        [HttpGet]
        public IEnumerable<Post> Get()
        {
            return repository.Get(p => p.Type != PostType.Update);
        }

        [HttpGet("{slug}")]
        public Post Get(string slug)
        {
            return repository.Get(slug);
        }
    }
}
