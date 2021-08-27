using System.Collections.Generic;
using System.Threading.Tasks;
using triggan.BlogModel;

namespace triggan.BlazorApp.Services
{
    public interface IBlogService
    {
        Task<List<T>> GetEntitiesAsync<T>(int count = 0) where T : Entity;

        Task<T> GetEntityAsync<T>(string slug) where T : Entity;

        Task<bool> PostOrUpdateEntityAsync<T>(string slug, T entity) where T : Entity;

        Task<bool> PostEntityAsync<T>(T entity) where T : Entity;

        Task<bool> UpdateEntityAsync<T>(string slug, T entity) where T : Entity;

        Task<bool> UpdateProjectAsync(string slug, Update update);

        Task<bool> StarEntity(string slug);
    }
}
