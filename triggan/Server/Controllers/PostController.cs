using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.Enums;
using DataAccessLayer.Interfaces;

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
        public IEnumerable<Post> Get([FromQuery] int count = 0)
        {
            return repository.Get(post => post.Type != PostType.Update, count: count);
        }

        [HttpGet("{slug}")]
        public Post Get(string slug)
        {
            return repository.Get(slug);
        }
    }
}
