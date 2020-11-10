using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using Model;
using System.Linq;
using Model.Enums;
using Microsoft.Azure.Cosmos;
using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;

namespace triggan.Functions
{
    public static class MessageFunctions
    {
        [FunctionName("Message")]
        public static async Task<IActionResult> SendMessage([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation($"Trying to save message to cosmos db");
            var content = await new StreamReader(req.Body).ReadToEndAsync();
            var message = JsonConvert.DeserializeObject<Message>(content);

            if (message == null)
            {
                return new UnprocessableEntityObjectResult(message);
            }

            var result = CosmosTools.AddMessage(message).Result;
            return result ? (ObjectResult)new OkObjectResult(message) : new UnprocessableEntityObjectResult(message);
        }
    }
}