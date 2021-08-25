using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Mail;
using triggan.BlogManager.Interfaces;
using triggan.BlogModel;

namespace triggan.API.Controllers
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
            repository.Add(message);
            repository.Save();
        }

        [HttpPost("[action]")]
        public void Contact(Message message)
        {
            using var mailMessage = new MailMessage();

            var user = config.GetValue<string>("MailCredentials:User");
            var password = config.GetValue<string>("MailCredentials:Password");

            mailMessage.From = new MailAddress("contact@triggan.com");
            mailMessage.ReplyToList.Add(new MailAddress(message.EMail, message.Name));
            mailMessage.To.Add(new MailAddress("tristan.robin69@gmail.com"));
            mailMessage.Subject = $"{message.Name} contacted you";
            mailMessage.IsBodyHtml = false;
            mailMessage.Body = message.Content;

            using var client = new SmtpClient("smtp.gmail.com", 587);

            client.Credentials = new System.Net.NetworkCredential(user, password);
            client.EnableSsl = true;
            client.Send(mailMessage);
        }
    }
}