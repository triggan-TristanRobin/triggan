using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using triggan.BlogManager;
using triggan.BlogManager.Interfaces;
using triggan.BlogModel;
using triggan.BlogModel.Enums;

namespace triggan.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private readonly BlogAccessor accessor;

        public PostController(BlogAccessor accessor)
        {
            this.accessor = accessor;
        }

        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<Post> Get([FromQuery] int count = 0)
        {
            return accessor.GetAll<Post>(count);
        }

        [AllowAnonymous]
        [HttpGet("{slug}")]
        public Post Get(string slug)
        {
            return accessor.Get(slug) as Post;
        }

        [HttpPost()]
        public Post Post(Post post)
        {
            return accessor.Add(post);
        }

        [HttpPut("{slug}")]
        public Post Put(string slug, Post post)
        {
            return accessor.Update(slug, post);
        }

        [HttpDelete("{slug}")]
        public Post Delete(string slug)
        {
            return accessor.Delete(slug) as Post;
        }
    }
}