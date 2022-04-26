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
    public class BookController : ControllerBase
    {
        private readonly BlogAccessor accessor;

        public BookController(BlogAccessor accessor)
        {
            this.accessor = accessor;
        }

        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<Book> Get([FromQuery] int count = 0)
        {
            return accessor.GetAll<Book>(count);
        }

        [AllowAnonymous]
        [HttpGet("{slug}")]
        public Book Get(string slug)
        {
            return accessor.Get(slug) as Book;
        }

        [HttpPost()]
        public Book Post(Book book)
        {
            return accessor.Add(book);
        }

        [HttpPut("{slug}")]
        public Book Put(string slug, Book book)
        {
            return accessor.Update(slug, book);
        }

        [HttpDelete("{slug}")]
        public Book Delete(string slug)
        {
            return accessor.Delete(slug) as Book;
        }
    }
}