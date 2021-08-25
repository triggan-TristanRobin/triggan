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

            var tmp = (await APIHttp.GetFromJsonAsync<IEnumerable<T>>(url));
            return (count > 0 ? tmp.Take(count) : tmp).ToList();
        }

        public async Task<T> GetEntityAsync<T>(string slug) where T : Entity
        {
            if(Settings.UseLocal)
            {
                return (await APIHttp.GetFromJsonAsync<IEnumerable<T>>(Settings.GetFullUrl(typeof(T).Name))).SingleOrDefault(e => e.Slug == slug);
            }
            var url = Settings.GetFullUrl($"{typeof(T).Name}", slug, queryParam: (typeof(T) == typeof(Project) ? $"withUpdates=true" : ""));
            Console.WriteLine($"Retrieving entity from {url}");

            return await APIHttp.GetFromJsonAsync<T>(url);
        }

        public async Task<bool> PostOrUpdateEntityAsync<T>(string slug, T entity) where T : Entity
        {
            return await (string.IsNullOrEmpty(slug) ? PostEntityAsync(entity) : UpdateEntityAsync(slug, entity));
        }

        public async Task<bool> PostEntityAsync<T>(T entity) where T : Entity
        {
            //var success = await APIHttp.PostAsJsonAsync($"Commit/{typeof(T).Name}/{Slug}", entity);
            var success = await APIHttp.PostAsJsonAsync(Settings.GetFullUrl($"{typeof(T).Name}"), entity);

            return success.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateEntityAsync<T>(string slug, T entity) where T : Entity
        {
            var success = await APIHttp.PutAsJsonAsync(Settings.GetFullUrl($"{typeof(T).Name}", slug), entity);

            return success.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateProjectAsync(string slug, Update update)
        {
            var success = await APIHttp.PostAsJsonAsync(Settings.GetFullUrl($"Project", slug, "Updates"), update);
            return success.IsSuccessStatusCode;
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