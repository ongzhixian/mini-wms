using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;
using Wms.Models.Bookstore;

namespace Wms.Extensions;

public static class MongoCollectionCategoryExtensions
{
    //public static IMongoCollection<Category> Map(this IEnumerable<Claim> claims, IDictionary<string, string> claimTypeMap)
    //{
    //    foreach (Claim claim in claims)
    //    {
    //        if (claimTypeMap.TryGetValue(claim.Type, out string? value))
    //        {
    //            yield return new Claim(value, claim.Value, claim.ValueType, claim.Issuer, claim.OriginalIssuer, claim.Subject);
    //        }
    //        else
    //        {
    //            yield return claim;
    //        }
    //    }
    //}

    public static async Task SetupIndexesAsync(this IMongoCollection<Category> categories)
    {
        var documentCursor = await categories.Indexes.ListAsync();

        if (documentCursor != null)
        {
            var indexes = await documentCursor.ToListAsync();

            if (indexes.Count <= 1)
            {
                var indexKeys = Builders<Category>.IndexKeys
                    .Ascending(m => m.Name);

                var indexOptions = new CreateIndexOptions { Unique = true };

                await categories.Indexes.CreateOneAsync(new CreateIndexModel<Category>(indexKeys, indexOptions));
            }
        }
    }

    public static async Task SeedAsync(this IMongoCollection<Category> categories, IMemoryCache cache)
    {
        if (await categories.CountDocumentsAsync(_ => true) > 0)
        {
            return;
        }

        List<Category> seedData = new List<Category>();

        using (StreamReader sr = new("./Data/pubs/categories.csv"))
        {
            while (!sr.EndOfStream)
            {
                string? line = sr.ReadLine();
                
                if (line == null)
                {
                    continue;
                }

                seedData.Add(new Category
                {
                    Name = line,
                });
            }
        }

        //    string[] catList = new string[]
        //    {
        //    "business", "psychology", "traditional-cooking", "modern-cooking", "computing"
        //    };

        //var categorySeedData = catList.Select(m => new Category
        //{
        //    //Id = m,
        //    Name = m
        //});

        await categories.InsertManyAsync(seedData);

        //foreach (var item in seedData)
        //{
        //    // <database>.<collection>.<key>
        //    cache.Set($"bookstore.{nameof(Category)}.{item.Name}", item);
        //}
    }

    
}
