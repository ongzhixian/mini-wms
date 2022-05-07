using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;
using Wms.Models.Bookstore;

namespace Wms.Extensions;

public static class MongoCollectionContactTypeExtensions
{
    //public static IMongoCollection<ContactType> Map(this IEnumerable<Claim> claims, IDictionary<string, string> claimTypeMap)
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

    public static async Task SetupIndexesAsync(this IMongoCollection<ContactType> categories)
    {
        var documentCursor = await categories.Indexes.ListAsync();

        if (documentCursor != null)
        {
            var indexes = await documentCursor.ToListAsync();

            if (indexes.Count <= 1)
            {
                var indexKeys = Builders<ContactType>.IndexKeys
                    .Ascending(m => m.Name);

                var indexOptions = new CreateIndexOptions { Unique = true };

                await categories.Indexes.CreateOneAsync(new CreateIndexModel<ContactType>(indexKeys, indexOptions));
            }
        }
    }

    public static async Task SeedAsync(this IMongoCollection<ContactType> categories, IMemoryCache cache)
    {
        if (await categories.CountDocumentsAsync(_ => true) > 0)
        {
            return;
        }

        List<ContactType> seedData = new List<ContactType>();

        using (StreamReader sr = new("./Data/pubs/contactTypes.csv"))
        {
            while (!sr.EndOfStream)
            {
                string? line = sr.ReadLine();
                
                if (line == null)
                {
                    continue;
                }

                seedData.Add(new ContactType
                {
                    Name = line,
                });
            }
        }

        await categories.InsertManyAsync(seedData);

    }
}
