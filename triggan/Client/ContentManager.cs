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
            var tmp = (await Http.GetFromJsonAsync<IEnumerable<T>>(Settings.GetFullUrl($"{typeof(T).Name}", count.ToString())));
            return (count > 0 ? tmp.Take(count) : tmp).ToList();
        }

        public async Task<T> GetEntityAsync<T>(string slug) where T : Entity
        {
            return (await Http.GetFromJsonAsync<IEnumerable<T>>(Settings.GetFullUrl(typeof(T).Name))).SingleOrDefault(e => e.Slug == slug);
        }

        public async Task<bool> PostEntityAsync<T>(T entity) where T : Entity
        {
            var success = await Http.PostAsJsonAsync($"Commit/{typeof(T).Name}/{entity.Slug}", entity);

            return success.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateProjectAsync(string slug, Update update)
        {
            var savedProject = await GetEntityAsync<Project>(slug);
            savedProject.Updates.Add(update);
            return await PostEntityAsync(savedProject);
        }

        public async Task<int> StarEntity<T>(string slug) where T : Entity
        {
            var entity = await GetEntityAsync<T>(slug);
            entity.Stars++;
            var success = await FunctionsHttp.GetAsync(Settings.GetFullUrl(typeof(T).Name, entity.Slug, "Star", local: false));

            Console.WriteLine("Star result: " + await success.Content.ReadAsStringAsync());
            return int.Parse(await success.Content.ReadAsStringAsync());
        }
    }
}
