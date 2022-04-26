using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using triggan.BlogModel;

namespace triggan.BlogManager
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

                string projectsAsJson = File.ReadAllText(@$"{currentPath}./Seeds/projects.json");
                var projects = JsonSerializer.Deserialize<List<Project>>(projectsAsJson);

                context.AddRange(projects);
                context.SaveChanges();
            }

            if(!context.Books.Any())
            {
                Trace.TraceInformation("Add commited books");

                var date = JsonSerializer.Serialize(new DateTime(2022, 04, 20));
                string booksAsJson = File.ReadAllText(@$"{currentPath}./Seeds/books.json");
                var books = JsonSerializer.Deserialize<List<Book>>(booksAsJson);

                context.AddRange(books);
                context.SaveChanges();
            }
        }
    }
}