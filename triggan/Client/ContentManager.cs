using Microsoft.AspNetCore.Components;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace triggan.Client
{
    public class ContentManager
    {
        public ContentManager(Settings settings, HttpClient httpClient)
        {
            Settings = settings;
            Http = httpClient;
        }

        public bool UseLocalDb => Settings?.UseLocal ?? false;
        protected Settings Settings { get; set; }
        protected HttpClient Http { get; }
        protected HttpClient FunctionsHttp { get; } = new HttpClient { BaseAddress = new Uri("https://trigganfunctions.azurewebsites.net") };

        public async Task<List<T>> GetEntitiesAsync<T>(int count = 0) where T : Entity
        {
            var list = await (UseLocalDb ? Http : FunctionsHttp).GetFromJsonAsync<List<T>>(Settings.GetFullUrl($"{typeof(T).Name}{(Settings.UseLocal ? "" : "s")}", count.ToString())); ;
            if(Settings.UseLocal)
            {
                list = list.Take(count).ToList();
            }
            return list;
        }

        public async Task<T> GetEntityAsync<T>(string slug) where T : Entity
        {
            T entity;
            var result = await (UseLocalDb ? Http : FunctionsHttp).GetAsync(Settings.GetFullUrl(typeof(T).Name, slug));
            entity = Settings.UseLocal ? (await result.Content.ReadFromJsonAsync<List<T>>()).SingleOrDefault(e => e.Slug == slug) : result as T;
            return entity;
        }

        public async Task<bool> PostEntityAsync<T>(T entity) where T : Entity
        {
            var success = await FunctionsHttp.PostAsJsonAsync(Settings.GetFullUrl(typeof(T).Name, entity.Slug, forceLocal: false), entity);
            if (success.IsSuccessStatusCode)
            {
                await WriteNewEntityToFile(entity);
            }

            return success.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateProjectAsync(string slug, Update update)
        {
            var success = await Http.PostAsJsonAsync(Settings.GetFullUrl("Project", slug, "Update", false), update);
            if (success.IsSuccessStatusCode)
            {
                var projects = await GetEntitiesAsync<Project>();
                Project oldProject;
                if ((oldProject = projects.SingleOrDefault(e => e.Slug == slug)) != null)
                {
                    oldProject.Updates.Add(update);
                }
                var serializedProjects = JsonSerializer.Serialize(projects, new JsonSerializerOptions
                {
                    WriteIndented = true,
                });
                using var streamWriter = new StreamWriter(Settings.GetFullUrl("Project", forceLocal: true));
                streamWriter.Write(serializedProjects);
            }

            return success.IsSuccessStatusCode;
        }

        public async Task<int> StarEntity<T>(string slug) where T : Entity
        {
            var entity = await GetEntityAsync<T>(slug);
            entity.Stars++;
            var success = await FunctionsHttp.GetAsync(Settings.GetFullUrl(typeof(T).Name, entity.Slug, "Star", forceLocal: false));
            if (success.IsSuccessStatusCode)
            {
                await WriteNewEntityToFile(entity);
            }
            Console.WriteLine("Star result: " + await success.Content.ReadAsStringAsync());
            return int.Parse(await success.Content.ReadAsStringAsync());
        }

        private async Task WriteNewEntityToFile<T>(T entity) where T : Entity
        {
            try
            {
                var entities = await GetEntitiesAsync<T>();
                T oldEntity;
                if ((oldEntity = entities.SingleOrDefault(e => e.Slug == entity.Slug)) != null)
                {
                    oldEntity = entity;
                }
                var serializedEntities = JsonSerializer.Serialize(entities, new JsonSerializerOptions
                {
                    WriteIndented = true,
                });
                Console.WriteLine("Trying shit");
                Console.WriteLine("Didn't fail?");
                Console.WriteLine(File.ReadAllText($"{Directory.GetCurrentDirectory()}{@"\wwwroot\Post\content.json"}"));
                Console.WriteLine("Didn't fail too?");
                using var sr = new StreamReader(Path.Combine(Http.BaseAddress.ToString(), Settings.GetFullUrl(typeof(T).Name, forceLocal: true)));
                Console.WriteLine("Current content: ");
                Console.WriteLine(sr.ReadToEnd());
                using var streamWriter = new StreamWriter(Path.Combine(Http.BaseAddress.ToString(), Settings.GetFullUrl(typeof(T).Name, forceLocal: true)));
                streamWriter.Write(serializedEntities);
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception with local writing: " + e.Message);
            }
        }
    }
}
