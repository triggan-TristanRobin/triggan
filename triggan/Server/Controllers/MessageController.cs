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
    public class MessageController : ControllerBase
    {
        private readonly IRepository<Message> repository;

        public MessageController(IRepository<Message> repo)
        {
            this.repository = repo;
        }

        [HttpGet]
        public IEnumerable<Message> Get()
        {
            return repository.Get();
        }

        [HttpPost]
        public void Insert(Message message)
        {
            repository.Insert(message);
            repository.Save();
        }
    }
}
