using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model;
using triggan.Interfaces;

namespace triggan.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IRepository<Post> repository;

        public PostController(IRepository<Post> repo)
        {
            this.repository = repo;
        }

        [HttpGet]
        public IEnumerable<Post> Get()
        {
            return repository.Get();
        }

        [HttpGet("{slug}")]
        public Post Get(string slug)
        {
            return repository.Get(slug);
        }
    }
}
