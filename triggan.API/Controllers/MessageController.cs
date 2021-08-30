using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Mail;
using triggan.BlogManager.Interfaces;
using triggan.BlogModel;

namespace triggan.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {

        private readonly IConfiguration config;

        public MessageController(IConfiguration config)
        {
            this.config = config;
        }

        [HttpPost("[action]")]
        public void Contact(Message message)
        {
            using var mailMessage = new MailMessage();

            var user = config.GetValue<string>("MailCredentials:User");
            var password = config.GetValue<string>("MailCredentials:Password");

            var sender = new MailAddress(message.EMail, message.Name);
            mailMessage.From = new MailAddress("contact@triggan.com");
            mailMessage.ReplyToList.Add(sender);
            mailMessage.To.Add(new MailAddress("tristan.robin69@gmail.com"));
            mailMessage.CC.Add(sender);
            mailMessage.Subject = $"{message.Name} contacted you on triggan";
            mailMessage.IsBodyHtml = false;
            mailMessage.Body = message.Content;

            using var client = new SmtpClient("smtp.gmail.com", 587);

            client.Credentials = new System.Net.NetworkCredential(user, password);
            client.EnableSsl = true;
            client.Send(mailMessage);
        }
    }
}