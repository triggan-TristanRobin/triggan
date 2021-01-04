using LibGit2Sharp;
using Microsoft.AspNetCore.Mvc;
using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace triggan.Server.Controllers
{
    public class GitController : Controller
    {
        private LibGit2Sharp.Credentials credentials = null;

        [HttpPost("[action]")]
        public void Authenticate([FromBody] Model.Credentials creds)
        {
            credentials = new UsernamePasswordCredentials
            {
                Username = creds.Id,
                Password = creds.Password
            };
        }

        [HttpPost("[action]/Post/{slug}")]
        public bool Commit(string slug, [FromBody] Post entity)
        {
            return Commit(slug, entity);
        }


        [HttpPost("[action]/Project/{slug}")]
        public bool Commit(string slug, [FromBody] Project entity)
        {
            return Commit(slug, entity);
        }

        public bool Commit<T>(string slug, [FromBody] T entity) where T: Entity
        {
            Console.WriteLine($"Trying git features");

            if (credentials == null)
            {
                return false;
            }

            var options = new CloneOptions
            {
                CredentialsProvider = (_url, _user, _cred) => credentials
            };
            string clonedRepoPath = Repository.Clone("https://github.com/triggan-TristanRobin/triggan", Path.Combine(Directory.GetCurrentDirectory(), "Clone"), options);
            Console.WriteLine($"Cloned here : {clonedRepoPath}");

            using (var repo = new Repository(clonedRepoPath))
            {
                Branch currentBranch = Commands.Checkout(repo, repo.Branches["publication"]);

                var contentPath = Path.Combine(clonedRepoPath, @"triggan\Client\wwwroot", typeof(T).Name, "content.json");
                var entities = JsonSerializer.Deserialize<List<T>>(System.IO.File.ReadAllText(contentPath));

                // Upsert the entity
                var oldEntity = entities.SingleOrDefault(e => e.Slug == entity.Slug);
                if (oldEntity != null)
                {
                    oldEntity = entity;
                }
                else
                {
                    entities.Add(entity);
                }

                // save file
                System.IO.File.WriteAllText(contentPath, JsonSerializer.Serialize(entities, new JsonSerializerOptions { WriteIndented = true }));

                // commit
                var author = new Signature("triggan", "writer@triggan.com", DateTimeOffset.Now);
                repo.Commit($"Wrote {typeof(T).Name} slugged {entity.Slug}", author, author);

                // push
                repo.Network.Push(currentBranch, new PushOptions { CredentialsProvider = (_url, _user, _cred) => credentials });
            }

            return true;
        }

        public void StageChanges(Repository repo)
        {
            try
            {
                var status = repo.RetrieveStatus();
                var filePaths = status.Modified.Select(mods => mods.FilePath).ToList();
                filePaths.ForEach(filePath => { repo.Index.Add(filePath); repo.Index.Write(); });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:RepoActions:StageChanges " + ex.Message);
            }
        }
    }
}
