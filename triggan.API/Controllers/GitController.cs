using LibGit2Sharp;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using triggan.BlogModel;

namespace triggan.API.Controllers
{
    public class GitController : Controller
    {
        private static LibGit2Sharp.Credentials credentials = null;

        [HttpPost("[controller]/[action]")]
        public void Authenticate([FromBody] triggan.BlogModel.Credentials creds)
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
            return Commit<Post>(slug, entity);
        }

        [HttpPost("[action]/Project/{slug}")]
        public bool Commit(string slug, [FromBody] Project entity)
        {
            return Commit<Project>(slug, entity);
        }

        [HttpGet("[action]")]
        public bool Sync()
        {
            Console.WriteLine($"Synchronisation of site content");

            if (credentials == null)
            {
                return false;
            }

            var options = new CloneOptions
            {
                CredentialsProvider = (_url, _user, _cred) => credentials
            };
#if DEBUG
            var clonePath = @"E:\Programs\triggan\Temp";
#else
            var clonePath = Path.Combine(Directory.GetCurrentDirectory(), "Temp");
#endif
            string clonedRepoPath = Repository.Clone("https://github.com/triggan-TristanRobin/triggan", clonePath, options);
            Console.WriteLine($"Repo cloned here : {clonedRepoPath}");

            using (var repo = new Repository(clonedRepoPath))
            {
                CheckoutBranch(repo, "publication");
                SyncContenFromeClone(clonePath);
            }
            var directory = Directory.CreateDirectory(clonePath);
            RemoveReadOnlyAttributes(directory);
            directory.Delete(true);

            return true;
        }

        private static void SyncContenFromeClone(string clonePath)
        {
            var postContentPath = Path.Combine(clonePath, @"triggan\Client\wwwroot\Post\content.json");
            var projectContentPath = Path.Combine(clonePath, @"triggan\Client\wwwroot\Project\content.json");
            var postContent = System.IO.File.ReadAllText(postContentPath);
            var projectContent = System.IO.File.ReadAllText(projectContentPath);

            System.IO.File.WriteAllText(@"wwwroot\Post\content.json", postContent);
            System.IO.File.WriteAllText(@"wwwroot\Project\content.json", projectContent);
        }

        public bool Commit<T>(string slug, [FromBody] T entity) where T : Entity
        {
            Console.WriteLine($"Committing publication data");

            if (credentials == null)
            {
                return false;
            }

            var options = new CloneOptions
            {
                CredentialsProvider = (_url, _user, _cred) => credentials
            };
#if DEBUG
            var clonePath = @"E:\Programs\triggan\Temp";
#else
            var clonePath = Path.Combine(Directory.GetCurrentDirectory(), "Temp");
#endif
            string clonedRepoPath = Repository.Clone("https://github.com/triggan-TristanRobin/triggan", clonePath, options);
            Console.WriteLine($"Cloned here : {clonedRepoPath}");

            using (var repo = new Repository(clonedRepoPath))
            {
                var currentBranch = CheckoutBranch(repo, "publication");

                var contentPath = Path.Combine(clonePath, @"triggan\Client\wwwroot", typeof(T).Name, "content.json");
                var entities = JsonSerializer.Deserialize<List<T>>(System.IO.File.ReadAllText(contentPath));

                // Upsert the entity
                var oldEntity = entities.SingleOrDefault(e => e.Slug == slug);
                if (oldEntity != null)
                {
                    entity.Updated = DateTime.Now;
                    entities[entities.IndexOf(oldEntity)] = entity;
                }
                else
                {
                    entities.Add(entity);
                }

                // save file
                System.IO.File.WriteAllText(contentPath, JsonSerializer.Serialize(entities, new JsonSerializerOptions { WriteIndented = true }));

                // commit
                StageChanges(repo);
                var author = new Signature("triggan", "writer@triggan.com", DateTimeOffset.Now);
                repo.Commit($"Wrote {typeof(T).Name} slugged {slug}", author, author);

                // push
                repo.Network.Push(currentBranch, new PushOptions { CredentialsProvider = (_url, _user, _cred) => credentials });

                SyncContenFromeClone(clonedRepoPath);
            }
            var directory = Directory.CreateDirectory(clonePath);
            RemoveReadOnlyAttributes(directory);
            directory.Delete(true);

            return true;
        }

        public void RemoveReadOnlyAttributes(DirectoryInfo directory, bool recursive = true)
        {
            directory.Attributes = directory.Attributes & ~FileAttributes.ReadOnly;

            if (recursive)
            {
                var files = directory.GetFiles();
                var subDirs = directory.GetDirectories();
                Array.ForEach(files, (f) =>
                {
                    System.IO.File.SetAttributes(f.FullName, FileAttributes.Normal);
                });
                Array.ForEach(subDirs, d => RemoveReadOnlyAttributes(d));
            }
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

        private static Branch CheckoutBranch(Repository repo, string branchName)
        {
            var remotePublicationBranch = repo.Branches[$"refs/remotes/origin/{branchName}"];
            var localbranch = repo.CreateBranch("local", remotePublicationBranch.Tip);
            var updatedBranch = repo.Branches.Update(localbranch,
                    b => { b.TrackedBranch = remotePublicationBranch.CanonicalName; });
            return Commands.Checkout(repo, localbranch);
        }
    }
}