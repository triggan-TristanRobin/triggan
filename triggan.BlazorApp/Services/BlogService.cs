using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using triggan.BlogModel;

namespace triggan.BlazorApp.Services
{
    public class BlogService : IBlogService
    {
        public BlogService(Settings settings, HttpClient httpClient)
        {
            Settings = settings;
            APIClient = httpClient;
        }

        protected Settings Settings { get; set; }
        protected HttpClient APIClient { get; }

        public async Task<List<T>> GetEntitiesAsync<T>(int count = 0) where T : Entity
        {
            var url = Settings.GetFullUrl($"{typeof(T).Name}", queryParam: $"count={count}");
            Console.WriteLine($"Retrieving entities from {url}");

            var tmp = (await APIClient.GetFromJsonAsync<IEnumerable<T>>(url));
            return (count > 0 ? tmp.Take(count) : tmp).ToList();
        }

        public async Task<T> GetEntityAsync<T>(string slug) where T : Entity
        {
            if(Settings.UseLocal)
            {
                return (await APIClient.GetFromJsonAsync<IEnumerable<T>>(Settings.GetFullUrl(typeof(T).Name))).SingleOrDefault(e => e.Slug == slug);
            }
            var url = Settings.GetFullUrl($"{typeof(T).Name}", slug);
            Console.WriteLine($"Retrieving entity from {url}");

            return await APIClient.GetFromJsonAsync<T>(url);
        }

        public async Task<bool> PostOrUpdateEntityAsync<T>(string slug, T entity) where T : Entity
        {
            return await (string.IsNullOrEmpty(slug) ? PostEntityAsync(entity) : UpdateEntityAsync(slug, entity));
        }

        public async Task<bool> PostEntityAsync<T>(T entity) where T : Entity
        {
            //var success = await APIHttp.PostAsJsonAsync($"Commit/{typeof(T).Name}/{Slug}", entity);
            var success = await APIClient.PostAsJsonAsync(Settings.GetFullUrl($"{typeof(T).Name}"), entity);

            return success.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateEntityAsync<T>(string slug, T entity) where T : Entity
        {
            var success = await APIClient.PutAsJsonAsync(Settings.GetFullUrl($"{typeof(T).Name}", slug), entity);

            return success.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateProjectAsync(string slug, Update update)
        {
            var success = await APIClient.PostAsJsonAsync(Settings.GetFullUrl($"Project", route: $"{slug}/Updates"), update);
            return success.IsSuccessStatusCode;
        }

        public async Task<bool> StarEntity(string slug)
        {
            if(Settings.UseLocal)
            {
                return false;
            }

            var success = await APIClient.PostAsync($"{slug}/Star", null);
            return success.IsSuccessStatusCode;
        }
    }
}