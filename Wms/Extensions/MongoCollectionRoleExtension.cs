using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;
using Wms.Models.Shared;

namespace Wms.Extensions;

public static class MongoCollectionRoleExtensions
{
    public static async Task SetupIndexesAsync(this IMongoCollection<Role> categories)
    {
        var documentCursor = await categories.Indexes.ListAsync();

        if (documentCursor != null)
        {
            var indexes = await documentCursor.ToListAsync();

            if (indexes.Count <= 1)
            {
                var indexKeys = Builders<Role>.IndexKeys
                    .Ascending(m => m.Name);

                var indexOptions = new CreateIndexOptions { Unique = true };

                await categories.Indexes.CreateOneAsync(new CreateIndexModel<Role>(indexKeys, indexOptions));
            }
        }
    }

    public static async Task SeedAsync(this IMongoCollection<Role> Roles, IMemoryCache cache)
    {
        if (await Roles.CountDocumentsAsync(_ => true) > 0)
        {
            return;
        }

        List<Role> seedData = new List<Role>();

        using (StreamReader sr = new("./Data/shared/roles.csv"))
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

                // name,description
                const int NAME = 0;
                const int DESCRIPTION = 1;

                seedData.Add(new Role
                {
                    Name = fields[NAME],
                    Description = fields[DESCRIPTION],
                });
            }
        }

        await Roles.InsertManyAsync(seedData);
    }
}
