using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using triggan.Shared;

namespace triggan.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BlogController : ControllerBase
    {   
        private readonly ILogger<BlogController> _logger;

        public BlogController(ILogger<BlogController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Post> Get()
        {
            return new List<Post>
            {
                new Post
                {
                    Title = "First Post",
                    Content = "This is the first blog post with default content",
                    Excerpt = "This is the first blog post with default Excerpt",
                    CoverImagePath = "https://28oa9i1t08037ue3m1l0i861-wpengine.netdna-ssl.com/wp-content/uploads/2020/09/FEATURE-site-782x530.png",
                    Public = true,
                    PublicationDate = DateTime.Now,
                    Tags = new List<string> { "Tag1", "Tag2" },
                },
                new Post
                {
                    Title = "Second Post",
                    Content = "This is the Second blog post with default content",
                    Excerpt = "This is the Second blog post with default Excerpt",
                    CoverImagePath = "https://28oa9i1t08037ue3m1l0i861-wpengine.netdna-ssl.com/wp-content/uploads/2020/09/FEATURE-site-782x530.png",
                    Public = true,
                    PublicationDate = DateTime.Now,
                    Tags = new List<string> { "Tag1", "Tag2" },
                },
            };
        }
    }
}
