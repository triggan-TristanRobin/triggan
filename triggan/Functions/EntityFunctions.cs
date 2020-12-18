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
using Model;

namespace triggan.Functions
{
    public static class EntityFunctions
    {
        [FunctionName("Star")]
        public static async Task<IActionResult> Star([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "{type}/Star/{slug}")] HttpRequest req,
            string slug, string type, ILogger log)
        {
            log.LogInformation($"Trying to add a star to a {type}");
            var concreteType = typeof(Entity).Assembly.GetType($"Model.{type}");
            var result = -1;
            if(concreteType.IsSubclassOf(typeof(Entity)))
            {
                var task = (Task)typeof(CosmosTools).GetMethod("StarEntity").MakeGenericMethod(concreteType).Invoke(null, new object[] { slug, log });
                await task.ConfigureAwait(false);
                result = (int)task.GetType().GetProperty("Result").GetValue(task);
            }

            return result != -1 ? new OkObjectResult(result) : new UnprocessableEntityObjectResult(result);
        }
    }
}