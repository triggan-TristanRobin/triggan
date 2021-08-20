using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DataAccessLayer
{
    public static class DbInitializer
    {
        public static void Initialize(TrigganContext context, string currentPath = null)
        {
            Trace.TraceInformation("Migrating DB");

            switch (context.Database.ProviderName)
            {
                case "Cosmos":
                    context.DbType = Helpers.DBProvider.Cosmos;
                break;
                case "SqlServer":
                default:
                    context.DbType = Helpers.DBProvider.MSSQL;
                break;
            }


            if (context.DbType == Helpers.DBProvider.MSSQL)
            {
                context.Database.Migrate();
#if DEBUG
                //context.Database.EnsureDeleted();
                //context.Database.EnsureCreated();
#endif
            }

            if (!context.Posts.Any())
            {
                Trace.TraceInformation("Add commited posts");

                string postsAsJson = File.ReadAllText(@$"{currentPath}./Seeds/posts.json");
                var posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson);

                context.AddRange(posts);
                context.SaveChanges();
            }

            if (!context.Projects.Any())
            {
                Trace.TraceInformation("Add commited projects");

                string postsAsJson = File.ReadAllText(@$"{currentPath}./Seeds/projects.json");
                var projects = JsonSerializer.Deserialize<List<Project>>(postsAsJson);

                context.AddRange(projects);
                context.SaveChanges();
            }
        }
    }
}
