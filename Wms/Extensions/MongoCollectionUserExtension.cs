using Microsoft.Extensions.Caching.Memory;
using MongoDB.Bson;
using MongoDB.Driver;
using Wms.Models.Shared;

namespace Wms.Extensions;

public static class MongoCollectionUserExtensions
{
    public static async Task<User> FindFindFirstOrDefaultAsync(this IMongoCollection<User> users, string username)
    {
        var filterBuilder = Builders<User>.Filter;

        var filter = filterBuilder.Regex(
            m => m.Username,
            new BsonRegularExpression($"/.*{username}.*/i"));

        var options = new FindOptions<User>
        {
            Limit = 10,
            Sort = Builders<User>.Sort.Ascending(m => m.Username)
        };

        var docCursor = await users.FindAsync(filter, options);

        return await docCursor.FirstOrDefaultAsync();
    }

    public static async Task SetupIndexesAsync(this IMongoCollection<User> categories)
    {
        var documentCursor = await categories.Indexes.ListAsync();

        if (documentCursor != null)
        {
            var indexes = await documentCursor.ToListAsync();

            if (indexes.Count <= 1)
            {
                var indexKeys = Builders<User>.IndexKeys
                    .Ascending(m => m.Username);

                var indexOptions = new CreateIndexOptions { Unique = true };

                await categories.Indexes.CreateOneAsync(new CreateIndexModel<User>(indexKeys, indexOptions));
            }
        }
    }

    public static async Task SeedAsync(this IMongoCollection<User> Users, IMemoryCache cache)
    {
        if (await Users.CountDocumentsAsync(_ => true) > 0)
        {
            return;
        }

        List<User> seedData = new List<User>();

        using (StreamReader sr = new("./Data/shared/users.csv"))
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

                if ((fields.Length <= 0) || (fields.Length < 3))
                {
                    continue;
                }

                // username,password,roles
                const int USERNAME = 0;
                const int PASSWORD = 1;
                const int ROLES = 2;

                seedData.Add(new User
                {
                    Username = fields[USERNAME],
                    Password = fields[PASSWORD],
                    Roles = parsedRoles(fields[ROLES])
                });
            }
        }

        await Users.InsertManyAsync(seedData);
    }

    private static IList<string> parsedRoles(string roles)
    {
        return roles
            .Split(';', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x).ToList();
    }
}
