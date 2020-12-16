using Microsoft.AspNetCore.Components;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace triggan.Client
{
    public class ContentManager
    {
        public ContentManager(Settings settings, HttpClient httpClient)
        {
            Settings = settings;
            Http = httpClient;
        }
        protected Settings Settings { get; set; }
        protected HttpClient Http { get; set; }

        public async Task<List<T>> GetEntitiesAsync<T>(int count) where T : Entity
        {
            Console.WriteLine($"Getting Entities ({typeof(T).Name})");
            return await Http.GetFromJsonAsync<List<T>>(Settings.GetFullUrl($"{typeof(T).Name}{(Settings.UseLocal ? "" : "s")}", count.ToString()));
        }

        public async Task<T> GetEntityAsync<T>(string slug) where T : Entity
        {
            Console.WriteLine($"Getting Entity ({typeof(T).Name}) with slug {slug}");
            T entity;
            var result = await Http.GetAsync(Settings.GetFullUrl(typeof(T).Name, slug));
            if (Settings.UseLocal)
            {
                entity = (await result.Content.ReadFromJsonAsync<List<T>>()).SingleOrDefault(e => e.Slug == slug);
            }
            else
            {
                entity = result as T;
            }
            return entity;
        }
    }
}
