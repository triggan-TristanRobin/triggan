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

namespace triggan.Functions
{
    public static class PostFunctions
    {
        [FunctionName("Post")]
        public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Post/{postCount:int?}")] HttpRequest req, int? postCount,
        [CosmosDB(
            databaseName:"triggandb",
            collectionName:"postContainer",
            ConnectionStringSetting = "trigganCosmos"
            )] IEnumerable<Post> postSet,
        ILogger log)
        {
            log.LogInformation("Data fetched from PostContainer");
            var result = postSet.Where(post => post.Type != PostType.Update).OrderBy(post => post.Updated);
            return new OkObjectResult(postCount != null && postCount > 0 ? result.Take(postCount.Value) : result);
        }
    }
}
