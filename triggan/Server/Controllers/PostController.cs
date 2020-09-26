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
    public class PostController : ControllerBase
    {   
        private readonly ILogger<PostController> _logger;

        public PostController(ILogger<PostController> logger)
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
                    Excerpt = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin a commodo ex. Etiam tempor lorem mattis risus convallis rhoncus. Nam in aliquet orci, id accumsan magna. Duis velit lectus, ullamcorper sit amet lacus eu, viverra efficitur quam.",
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
