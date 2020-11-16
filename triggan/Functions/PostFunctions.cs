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
    public static class PostFunctions
    {
        [FunctionName("Posts")]
        public static async Task<IActionResult> GetPosts(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Posts/{postCount:int?}")] HttpRequest req, int? postCount, ILogger log)
        {
            log.LogInformation($"Retrieving {postCount} posts");
            var posts = CosmosTools.GetEntities<Post>(postCount ?? 0, logger: log).Result;
            return posts.Any() ? (ObjectResult)new OkObjectResult(posts) : new NotFoundObjectResult(posts);
        }

        [FunctionName("Post")]
        public static async Task<IActionResult> GetPost(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Post/{slug}")] HttpRequest req, string slug, ILogger log)
        {
            log.LogInformation("Data fetched from PostContainer");
            var post = CosmosTools.GetEntity<Post>(slug, logger: log).Result;
            return post != null ? (ObjectResult)new OkObjectResult(post) : new NotFoundObjectResult(post);
        }

        [FunctionName("Write")]
        public static async Task<IActionResult> WritePost(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Write/{slug}")] HttpRequest req, string slug, ILogger log)
        {
            log.LogInformation($"Trying to save post to cosmos db");
            var content = await new StreamReader(req.Body).ReadToEndAsync();
            var post = JsonConvert.DeserializeObject<Post>(content);

            if (post == null)
            {
                return new UnprocessableEntityObjectResult(post);
            }

            var result = CosmosTools.UpsertEntity(post, logger: log).Result;
            return result ? (ObjectResult)new OkObjectResult(post) : new UnprocessableEntityObjectResult(post);
        }
    }
}