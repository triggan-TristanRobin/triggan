using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using triggan.BlogManager;
using triggan.BlogManager.Interfaces;
using triggan.BlogModel;
using triggan.BlogModel.Enums;

namespace triggan.API.Controllers
{
    [ApiController]
    [Route("")]
    public class EntityController
    {
        private readonly BlogAccessor accessor;

        public EntityController(BlogAccessor accessor)
        {
            this.accessor = accessor;
        }

        [HttpPost("{slug}/[action]")]
        public int Star(string slug)
        {
            return accessor.Star(slug);
        }
    }
}
