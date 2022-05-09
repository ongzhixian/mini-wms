using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;
using Wms.Models.Shared;

namespace Wms.Extensions;

public static class MongoCollectionApplicationExtensions
{
    public static async Task SetupIndexesAsync(this IMongoCollection<Application> categories)
    {
        var documentCursor = await categories.Indexes.ListAsync();

        if (documentCursor != null)
        {
            var indexes = await documentCursor.ToListAsync();

            if (indexes.Count <= 1)
            {
                var indexKeys = Builders<Application>.IndexKeys
                    .Ascending(m => m.Name);

                var indexOptions = new CreateIndexOptions { Unique = true };

                await categories.Indexes.CreateOneAsync(new CreateIndexModel<Application>(indexKeys, indexOptions));
            }
        }
    }

    public static async Task SeedAsync(this IMongoCollection<Application> Applications, IMemoryCache cache)
    {
        if (await Applications.CountDocumentsAsync(_ => true) > 0)
        {
            return;
        }

        List<Application> seedData = new List<Application>();

        using (StreamReader sr = new("./Data/shared/applications.csv"))
        {
            sr.ReadLine(); // Skip first line

            while (!sr.EndOfStream)
            {
                string? line = sr.ReadLine();
                
                if (line == null)
                {
                    continue;
                }

                var fields = line.Split(',', StringSplitOptions.None);

                if ((fields.Length <= 0) || (fields.Length < 2))
                {
                    continue;
                }

                // au_id,au_lname,au_fname,phone,address,city,state,zip,contract
                const int NAME = 0;
                const int DESCRIPTION = 1;

                seedData.Add(new Application
                {
                    Name = fields[NAME],
                    Description = fields[DESCRIPTION],
                });
            }
        }


        await Applications.InsertManyAsync(seedData);
    }
}
