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
            var projects = await CosmosTools.GetEntities<Project>(projectCount ?? 0, logger: log);
            return projects.Any() ? new OkObjectResult(projects) : new NotFoundObjectResult(projects);
        }

        [FunctionName("Project")]
        public static async Task<IActionResult> GetProject(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Project/{slug}")] HttpRequest req, string slug, ILogger log)
        {
            var project = await CosmosTools.GetEntity<Project>(slug, logger: log);
            return project != null ? new OkObjectResult(project) : new NotFoundObjectResult(project);
        }

        [FunctionName("Create")]
        public static async Task<IActionResult> CreateProject(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Project/{slug}")] HttpRequest req, string slug, ILogger log)
        {
            log.LogInformation($"Trying to save project to cosmos db");
            var content = await new StreamReader(req.Body).ReadToEndAsync();
            var project = JsonConvert.DeserializeObject<Project>(content);

            if (project == null)
            {
                return new UnprocessableEntityObjectResult(project);
            }

            var result = await CosmosTools.UpsertEntity(project, logger: log);
            return result ? new OkObjectResult(project) : new UnprocessableEntityObjectResult(project);
        }

        [FunctionName("Update")]
        public static async Task<IActionResult> UpdateProject(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Project/Update/{slug}")] HttpRequest req, string slug, ILogger log)
        {
            log.LogInformation($"Trying to set update to project in cosmos db");
            var content = await new StreamReader(req.Body).ReadToEndAsync();
            var project = await CosmosTools.GetEntity<Project>(slug, logger: log);
            var update = JsonConvert.DeserializeObject<Update>(content);

            if (update == null)
            {
                return new UnprocessableEntityObjectResult(update);
            }
            if (project == null)
            {
                return new UnprocessableEntityObjectResult(project);
            }

            update.Created = DateTime.Now;
            project.Updates.Add(update);

            var result = await CosmosTools.UpsertEntity(project, logger: log);
            return result ? new OkObjectResult(project) : new UnprocessableEntityObjectResult(project);
        }
    }
}
