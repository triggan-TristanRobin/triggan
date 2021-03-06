﻿using System;
using System.Net;
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
        private static CosmosClient cosmosClient = new CosmosClient("AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
            new CosmosClientOptions
            {
                SerializerOptions = new CosmosSerializationOptions()
                {
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                },
            });
#else
        private static CosmosClient cosmosClient = new CosmosClient(Environment.GetEnvironmentVariable("trigganCosmos"),
            new CosmosClientOptions
            {
                SerializerOptions = new CosmosSerializationOptions()
                {
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                }
            });
#endif

        public async static Task<List<T>> GetEntities<T>(int count, string filter = "", ILogger logger = null) where T : Entity
        {
            var entities = new List<T>();
            var getAllQuery = $"SELECT * FROM c WHERE c.public = true";
            if (count > 0)
            {
                getAllQuery += $" OFFSET 0 LIMIT {count}";
            }
            logger?.LogInformation($"Get entities for {typeof(T).Name}");

            var container = cosmosClient.GetContainer("triggandb", $"{typeof(T).Name.ToLower()[0] + typeof(T).Name.Substring(1)}s");
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
            var getAllQuery = $"SELECT * FROM c WHERE c.slug = '{slug}'";
            logger?.LogInformation($"Get entity for {typeof(T).Name} ({slug})");

            var container = cosmosClient.GetContainer("triggandb", $"{typeof(T).Name.ToLower()[0] + typeof(T).Name.Substring(1)}s");
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

        public async static Task<int> StarEntity<T>(string slug, ILogger logger) where T : Entity
        {
            var entity = await GetEntity<T>(slug, logger);
            entity.Stars++;
            return await UpsertEntity<T>(entity, logger) ? entity.Stars : -1;
        }

        public async static Task<bool> UpsertEntity<T>(T entity, ILogger logger = null) where T : Entity
        {
            try
            {
                var container = cosmosClient.GetContainer("triggandb", $"{typeof(T).Name.ToLower()[0] + typeof(T).Name.Substring(1)}s");
                ItemResponse<T> result;
                if(string.IsNullOrEmpty(entity.Id))
                {
                    entity.Id = Guid.NewGuid().ToString();
                    result = await container.CreateItemAsync(entity, new PartitionKey(entity.Id));
                }
                else
                {
                    result = await container.UpsertItemAsync(entity);
                }

                logger?.LogInformation($"Upserted entity{typeof(T).Name} ({entity.Slug}) in database with id: {result.Resource.Id} Operation consumed {result.RequestCharge} RUs, Returned {result.StatusCode}.\n");
                return result.StatusCode == (HttpStatusCode.Created | HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                logger?.LogError("And exception occurred while adding an entity");
                logger?.LogError(e.Message);
                throw;
            }
        }

        public async static Task<bool> AddMessage(Message message)
        {
            try
            {
                var container = cosmosClient.GetContainer("triggandb", "messages");
                message.Id = Guid.NewGuid().ToString();
                var result = await container.CreateItemAsync(message, new PartitionKey(message.Id));
                Console.WriteLine($"Created item in database with id: {result.Resource.Id} Operation consumed {result.RequestCharge} RUs.\n");
                return result.StatusCode == (HttpStatusCode.Created | HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
