using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using triggan.BlazorApp.Helpers;
using triggan.BlogModel;

namespace triggan.BlazorApp
{
    public class ContentManager
    {
        public ContentManager(Settings settings, HttpClient httpClient)
        {
            Settings = settings;
            LocalHttp = httpClient;
            var apiUri = (settings.APIUri == null ? httpClient.BaseAddress : new Uri(settings.APIUri)).SetPort(settings.APIPort);
            Console.WriteLine($"Port in settings {settings.APIPort}");
            APIHttp = new HttpClient { BaseAddress = apiUri };
        }

        protected Settings Settings { get; set; }
        protected HttpClient LocalHttp { get; }
        protected HttpClient APIHttp { get; }
        protected HttpClient FunctionsHttp { get; } = new HttpClient { BaseAddress = new Uri("https://trigganfunctions.azurewebsites.net") };

        public async Task<List<T>> GetEntitiesAsync<T>(int count = 0) where T : Entity
        {
            var url = Settings.GetFullUrl($"{typeof(T).Name}", queryParam: $"count={count}");

            Console.WriteLine($"Retrieving entities from {url}");
            var fullTextReply = await APIHttp.GetAsync(url);
            Console.WriteLine(fullTextReply.StatusCode);
            Console.WriteLine(await fullTextReply.Content.ReadAsStringAsync());

            var tmp = (await APIHttp.GetFromJsonAsync<IEnumerable<T>>(url));
            return (count > 0 ? tmp.Take(count) : tmp).ToList();
        }

        public async Task<T> GetEntityAsync<T>(string slug) where T : Entity
        {
            return (await APIHttp.GetFromJsonAsync<IEnumerable<T>>(Settings.GetFullUrl(typeof(T).Name))).SingleOrDefault(e => e.Slug == slug);
        }

        public async Task<bool> PostEntityAsync<T>(string Slug, T entity) where T : Entity
        {
            var success = await APIHttp.PostAsJsonAsync($"Commit/{typeof(T).Name}/{Slug}", entity);

            return success.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateProjectAsync(string slug, Update update)
        {
            var savedProject = await GetEntityAsync<Project>(slug);
            savedProject.SetUpdate(update);
            return await PostEntityAsync(slug, savedProject);
        }

        public async Task<int> StarEntity<T>(string slug) where T : Entity
        {
            var entity = await GetEntityAsync<T>(slug);
            entity.Stars++;
            var success = await APIHttp.GetAsync(Settings.GetFullUrl(typeof(T).Name, entity.Slug, "Star", local: false));

            Console.WriteLine("Star result: " + await success.Content.ReadAsStringAsync());
            return int.Parse(await success.Content.ReadAsStringAsync());
        }
    }
}