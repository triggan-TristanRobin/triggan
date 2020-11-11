using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Model;
using System.Linq;
using Microsoft.Azure.Cosmos;
using System.Configuration;
using Microsoft.Extensions.Logging;

namespace triggan.Functions
{
    public static class CosmosTools
    {
#if DEBUG
        private static CosmosClient cosmosClient = new CosmosClient("AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");
#else
        private static CosmosClient cosmosClient = new CosmosClient(Environment.GetEnvironmentVariable("trigganCosmos"));
#endif

        public async static Task<List<T>> GetEntities<T>(int count, string filter = "", ILogger logger = null) where T : Entity
        {
            var entities = new List<T>();
            var getAllQuery = $"SELECT * FROM c WHERE c.Discriminator = '{typeof(T).Name}'";
            if (count > 0)
            {
                getAllQuery += $" OFFSET 0 LIMIT {count}";
            }
            logger?.LogInformation($"Get entities for {typeof(T).Name}");

            var container = cosmosClient.GetContainer("triggandb", "entities");
            QueryDefinition queryDefinition = new QueryDefinition(getAllQuery);

            var feedIterator = container.GetItemQueryIterator<T>(queryDefinition);
            while (feedIterator.HasMoreResults)
            {
                logger?.LogInformation($"Getting iterator results");
                foreach (var entity in await feedIterator.ReadNextAsync())
                {
                    logger?.LogInformation($"Adding entity {entity.Slug} to result list");
                    entities.Add(entity);
                }
            }

            return entities;
        }

        public async static Task<T> GetEntity<T>(string slug, ILogger logger = null) where T : Entity
        {
            var getAllQuery = $"SELECT * FROM c WHERE c.Slug = '{slug}'";
            logger?.LogInformation($"Get entity for {typeof(T).Name} ({slug})");

            var container = cosmosClient.GetContainer("triggandb", "entities");
            QueryDefinition queryDefinition = new QueryDefinition(getAllQuery);

            var iterator = container.GetItemQueryIterator<T>(queryDefinition);
            var entities = new List<T>();
            var feedIterator = container.GetItemQueryIterator<T>(queryDefinition);
            while (feedIterator.HasMoreResults)
            {
                logger?.LogInformation($"Getting iterator results");
                foreach (var entity in await feedIterator.ReadNextAsync())
                {
                    logger?.LogInformation($"Adding entity {entity.Slug} to result list");
                    entities.Add(entity);
                }
            }

            logger?.LogInformation($"Found {entities.Count()} results");
            return entities.SingleOrDefault();
        }

        public async static Task<bool> AddMessage(Message message)
        {
            try
            {
                var container = cosmosClient.GetContainer("triggandb", "messages");
                var result = await container.CreateItemAsync(message);
                Console.WriteLine("Created item in database with id: {0} Operation consumed {1} RUs.\n"
                    , result.Resource.Id, result.RequestCharge);
                return result.StatusCode.ToString().StartsWith("2");
            }
            catch(Exception e)
            {
                throw;
            }
        }
    }
}
