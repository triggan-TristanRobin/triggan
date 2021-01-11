using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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

        private readonly IConfiguration config;

        public MessageController(IRepository<Message> repo, IConfiguration config)
        {
            this.repository = repo;
            this.config = config;
        }

        [HttpGet]
        public IEnumerable<Message> Get()
        {
            return repository.GetAll();
        }

        [HttpPost]
        public void Insert(Message message)
        {
            repository.Insert(message);
            repository.Save();
        }

        [HttpPost("[action]")]
        public void Contact(Message message)
        {
            using (var mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress("contact@triggan.com");
                mailMessage.ReplyToList.Add(new MailAddress(message.EMail, message.Name));
                mailMessage.To.Add(new MailAddress("tristan.robin69@gmail.com"));
                mailMessage.Subject = $"{message.Name} contacted you";
                mailMessage.IsBodyHtml = false;
                mailMessage.Body = message.Content;
                using (var client = new SmtpClient("smtp.gmail.com", 587))
                {
                    var user = config.GetValue<string>("MailCredentials:User");
                    var password = config.GetValue<string>("MailCredentials:Password");
                    client.Credentials = new System.Net.NetworkCredential(user, password);
                    client.EnableSsl = true;
                    client.Send(mailMessage);
                }
            }
        }
    }
}
