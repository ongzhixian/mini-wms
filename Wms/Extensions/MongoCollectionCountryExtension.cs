using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;
using Wms.Models.Bookstore;

namespace Wms.Extensions;

public static class MongoCollectionCountryExtensions
{
    public static async Task SetupIndexesAsync(this IMongoCollection<Country> countries)
    {
        var documentCursor = await countries.Indexes.ListAsync();

        if (documentCursor != null)
        {
            var indexes = await documentCursor.ToListAsync();

            if (indexes.Count <= 1)
            {
                var indexKeys = Builders<Country>.IndexKeys
                    .Ascending(m => m.Alpha3);

                var indexOptions = new CreateIndexOptions { Unique = true };

                await countries.Indexes.CreateOneAsync(new CreateIndexModel<Country>(indexKeys, indexOptions));
            }
        }
    }

    public static async Task SeedAsync(this IMongoCollection<Country> countries, IMemoryCache cache)
    {
        if (await countries.CountDocumentsAsync(_ => true) > 0)
        {
            return;
        }

        List<Country> seedData = new List<Country>();

        using (StreamReader sr = new("./Data/pubs/unsd-m49.csv"))
        {
            sr.ReadLine(); // Skip first line

            while (!sr.EndOfStream)
            {
                string? line = sr.ReadLine();
                
                if (line == null)
                {
                    continue;
                }

                var fields = line.Split(';', StringSplitOptions.None);

                if ((fields.Length <= 0) || (fields.Length < 15))
                {
                    continue;
                }

                // Global Code;Global Name;Region Code;Region Name;Sub-region Code;Sub-region Name;Intermediate Region Code;Intermediate Region Name;Country or Area;M49 Code;ISO-alpha2 Code;ISO-alpha3 Code;Least Developed Countries (LDC);Land Locked Developing Countries (LLDC);Small Island Developing States (SIDS)
                //const int GLOBAL_CODE = 0;
                //const int GLOBAL_NAME = 1;
                //const int REGION_CODE = 2;
                //const int REGION_NAME = 3;
                //const int SUB_REGION_CODE = 4;
                //const int SUB_REGION_NAME = 5;
                //const int INTERMEDIATE_REGION_CODE = 6;
                //const int INTERMEDIATE_REGION_NAME = 7;
                const int COUNTRY_OR_AREA = 8;
                //const int M49_CODE = 9;
                const int ISO_ALPHA2_CODE = 10;
                const int ISO_ALPHA3_CODE = 11;
                //const int LEAST_DEVELOPED_COUNTRIES_LDC = 12;
                //const int LAND_LOCKED_DEVELOPING_COUNTRIES_LLDC = 13;
                //const int SMALL_ISLAND_DEVELOPING_STATES_SIDS = 14;

                seedData.Add(new Country
                {
                    Name = fields[COUNTRY_OR_AREA],
                    Alpha2 = fields[ISO_ALPHA2_CODE],
                    Alpha3 = fields[ISO_ALPHA3_CODE],
                });
            }
        }

        await countries.InsertManyAsync(seedData);

        //foreach (var item in seedData)
        //{
        //    // <database>.<collection>.<key>
        //    cache.Set($"bookstore.{nameof(Category)}.{item.Name}", item);
        //}
    }

    
}
