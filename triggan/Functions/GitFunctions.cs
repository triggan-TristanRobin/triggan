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
using LibGit2Sharp;

namespace triggan.Functions
{
    public static class GitFunctions
    {
        [FunctionName("Test")]
        public static async Task<IActionResult> Test([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation($"Trying git features");

            string clonedRepoPath = Repository.Clone("http://github.com/libgit2/TestGitRepository", Directory.GetCurrentDirectory());
            log.LogInformation($"Cloned here : {clonedRepoPath}");

            using (var repo = new Repository("http://github.com/libgit2/TestGitRepository"))
            {
                Commit commit = repo.Lookup<Commit>("73b48894238c3e9c37f9f3a696bbd4bffcf45ce5");
                log.LogInformation("Author: {0}", commit.Author.Name);
                log.LogInformation("Message: {0}", commit.MessageShort);
            }

            using (var repo = new Repository(clonedRepoPath))
            {
                Commit commit = repo.Lookup<Commit>("73b48894238c3e9c37f9f3a696bbd4bffcf45ce5");
                log.LogInformation("Author: {0}", commit.Author.Name);
                log.LogInformation("Message: {0}", commit.MessageShort);
            }

            return new OkResult();
        }
    }
}