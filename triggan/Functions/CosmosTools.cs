using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Model;
using System.Linq;
using Microsoft.Azure.Cosmos;
using System.Configuration;

namespace triggan.Functions
{
    public static class CosmosTools
    {
#if DEBUG
        private static CosmosClient cosmosClient = new CosmosClient("https://localhost:8081", "C2y6yDjf5/Rob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");
#else
        private static CosmosClient cosmosClient = new CosmosClient(ConfigurationManager.ConnectionStrings["trigganCosmos"].ConnectionString);
#endif

        public async static Task<List<T>> GetEntities<T>(int count, string filter = "") where T : Entity
        {
            var getAllQuery = $"SELECT * FROM c WHERE c.Discriminator = '{typeof(T).Name}'";
            if (count > 0)
            {
                getAllQuery += $" OFFSET 0 LIMIT {count}";
            }
            var container = cosmosClient.GetContainer("triggandb", "entities");
            QueryDefinition queryDefinition = new QueryDefinition(getAllQuery);

            var iterator = container.GetItemQueryIterator<T>(queryDefinition);
            var entities = new List<T>();
            var feedIterator = container.GetItemQueryIterator<T>(queryDefinition);
            while (feedIterator.HasMoreResults)
            {
                foreach (var entity in await feedIterator.ReadNextAsync())
                {
                    entities.Add(entity);
                }
            }

            return entities;
        }

        public async static Task<T> GetEntity<T>(string slug) where T : Entity
        {
            var getAllQuery = $"SELECT * FROM c WHERE c.Slug = '{slug}'";
            var container = cosmosClient.GetContainer("triggandb", "entities");
            QueryDefinition queryDefinition = new QueryDefinition(getAllQuery);

            var iterator = container.GetItemQueryIterator<T>(queryDefinition);
            var entities = new List<T>();
            var feedIterator = container.GetItemQueryIterator<T>(queryDefinition);
            while (feedIterator.HasMoreResults)
            {
                foreach (var entity in await feedIterator.ReadNextAsync())
                {
                    entities.Add(entity);
                }
            }

            return entities.SingleOrDefault();
        }
    }
}
