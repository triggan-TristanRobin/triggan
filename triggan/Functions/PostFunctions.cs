using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Model;

namespace triggan.Functions
{
    public static class PostFunctions
    {
        [FunctionName("Post")]
        public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
        [CosmosDB(
            databaseName:"TRIGGANDB",
            collectionName:"trigganContainer",
            ConnectionStringSetting = "trigganCosmos"
            )] IEnumerable<Post> postSet,
        ILogger log)
        {
            log.LogInformation("Data fetched from FAQContainer");
            return new OkObjectResult(postSet);
        }
    }
}
