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
        [FunctionName("Posts")]
        public static async Task<IActionResult> GetPosts(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Posts/{postCount:int?}")] HttpRequest req, int? postCount,
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

        [FunctionName("Post")]
        public static async Task<IActionResult> GetPost(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Post/{id:int}")] HttpRequest req, int id,
        [CosmosDB(
            databaseName:"triggandb",
            collectionName:"postContainer",
            ConnectionStringSetting = "trigganCosmos"
            )] IEnumerable<Post> postSet,
        ILogger log)
        {
            log.LogInformation("Data fetched from PostContainer");
            var result = postSet.SingleOrDefault(post => post.Id == id && post.Type != PostType.Update);
            return result != null ? (ObjectResult)new OkObjectResult(result) : new NotFoundObjectResult(result);
        }
    }
}
