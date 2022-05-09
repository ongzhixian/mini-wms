using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;
using Wms.Models.Bookstore;

namespace Wms.Extensions;

public static class MongoCollectionTagExtension
{
    //public static IMongoCollection<Tag> Map(this IEnumerable<Claim> claims, IDictionary<string, string> claimTypeMap)
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

    public static async Task SetupIndexesAsync(this IMongoCollection<Models.Bookstore.Tag> tags)
    {
        var documentCursor = await tags.Indexes.ListAsync();

        if (documentCursor != null)
        {
            var indexes = await documentCursor.ToListAsync();

            if (indexes.Count <= 1)
            {
                var indexKeys = Builders<Models.Bookstore.Tag>.IndexKeys
                    .Ascending(m => m.Name);

                var indexOptions = new CreateIndexOptions { Unique = true };

                await tags.Indexes.CreateOneAsync(new CreateIndexModel<Models.Bookstore.Tag>(indexKeys, indexOptions));
            }
        }
    }

    public static async Task SeedAsync(this IMongoCollection<Models.Bookstore.Tag> tags, IMemoryCache cache)
    {
        if (await tags.CountDocumentsAsync(_ => true) > 0)
        {
            return;
        }

        List<Models.Bookstore.Tag> seedData = new List<Models.Bookstore.Tag>();

        using (StreamReader sr = new("./Data/pubs/tags.csv"))
        {
            while (!sr.EndOfStream)
            {
                string? line = sr.ReadLine();
                
                if (line == null)
                {
                    continue;
                }

                seedData.Add(new Models.Bookstore.Tag
                {
                    Name = line,
                });
            }
        }

        //    string[] catList = new string[]
        //    {
        //    "business", "psychology", "traditional-cooking", "modern-cooking", "computing"
        //    };

        //var TagSeedData = catList.Select(m => new Tag
        //{
        //    //Id = m,
        //    Name = m
        //});

        await tags.InsertManyAsync(seedData);

        //foreach (var item in seedData)
        //{
        //    // <database>.<collection>.<key>
        //    cache.Set($"bookstore.{nameof(Tag)}.{item.Name}", item);
        //}
    }

    
}
