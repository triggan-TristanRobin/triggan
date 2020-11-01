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
    public static class ProjectFunctions
    {
        [FunctionName("Projects")]
        public static async Task<IActionResult> GetProjects(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Projects/{projectCount:int?}")] HttpRequest req, int? projectCount,
        [CosmosDB(
            databaseName:"triggandb",
            collectionName:"projectContainer",
            ConnectionStringSetting = "trigganCosmos"
            )] IEnumerable<Project> projectSet,
        ILogger log)
        {
            log.LogInformation("Data fetched from PostContainer");
            var result = projectSet.OrderBy(p => p.Updated);
            return new OkObjectResult(projectCount != null && projectCount > 0 ? result.Take(projectCount.Value) : result);
        }

        [FunctionName("Project")]
        public static async Task<IActionResult> GetProject(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Project/{id:int}")] HttpRequest req, int id,
        [CosmosDB(
            databaseName:"triggandb",
            collectionName:"projectContainer",
            ConnectionStringSetting = "trigganCosmos"
            )] IEnumerable<Project> projectSet,
        ILogger log)
        {
            log.LogInformation("Data fetched from PostContainer");
            var result = projectSet.SingleOrDefault(p => p.Id == id);
            return result != null ? (ObjectResult)new OkObjectResult(result) : new NotFoundObjectResult(result);
        }
    }
}
