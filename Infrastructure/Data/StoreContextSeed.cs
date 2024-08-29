using Core.Entites;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext storeContext, ILogger<StoreContextSeed> logger)
        {
            try
            {
                if (!storeContext.Products.Any())
                {
                    var path = Path.Combine(AppContext.BaseDirectory, "Data", "SeedData", "products.json");

                    if (!File.Exists(path))
                    {
                        logger.LogError($"Seed file not found: {path}");
                        return;
                    }

                    var productData = await File.ReadAllTextAsync(path);

                    var products = JsonSerializer.Deserialize<List<Product>>(productData);

                    if (products == null)
                    {
                        logger.LogWarning("No products to seed, deserialization returned null.");
                        return;
                    }

                    storeContext.AddRange(products);
                    await storeContext.SaveChangesAsync();

                    logger.LogInformation("Database has been seeded with products.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }

        public static async Task CheckDatabaseConnection(StoreContext storeContext, ILogger<StoreContextSeed> logger)
        {
            try
            {
                if (storeContext.Database.CanConnect())
                {
                    logger.LogInformation("Successfully connected to database");
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning("Can't connect to database");
            }
        }
    }
}