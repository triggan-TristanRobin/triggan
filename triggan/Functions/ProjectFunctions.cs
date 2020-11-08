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

namespace triggan.Functions
{
    public static class ProjectFunctions
    {
        [FunctionName("Projects")]
        public static async Task<IActionResult> GetProjects(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Projects/{projectCount:int?}")] HttpRequest req, int? projectCount, ILogger log)
        {
            var projects = CosmosTools.GetEntities<Project>(projectCount ?? 0).Result;
            return projects.Any() ? (ObjectResult)new OkObjectResult(projects) : new NotFoundObjectResult(projects);
        }

        [FunctionName("Project")]
        public static async Task<IActionResult> GetProject(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Project/{slug}")] HttpRequest req, string slug, ILogger log)
        {
            var project = CosmosTools.GetEntity<Project>(slug).Result;
            return project != null ? (ObjectResult)new OkObjectResult(project) : new NotFoundObjectResult(project);
        }
    }
}
