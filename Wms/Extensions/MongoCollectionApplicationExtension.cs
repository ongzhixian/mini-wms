﻿using Microsoft.Extensions.Caching.Memory;
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
                    .Ascending(m => m.Id);

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

        using (StreamReader sr = new("./Data/pubs/Applications.csv"))
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

                if ((fields.Length <= 0) || (fields.Length < 9))
                {
                    continue;
                }

                // au_id,au_lname,au_fname,phone,address,city,state,zip,contract
                const int ID = 0;
                const int LAST_NAME = 1;
                const int FIRST_NAME = 2;
                const int PHONE = 3;
                const int ADDRESS = 4;
                const int CITY = 5;
                const int STATE = 6;
                const int ZIP = 7;
                const int CONTRACT = 8;

                // seedData.Add(new Application
                // {
                //     Id = fields[ID],
                //     FirstName = fields[FIRST_NAME],
                //     LastName = fields[LAST_NAME],
                //     Contract = fields[CONTRACT] == "1" ? true : false,
                //     Contacts = new List<Contact>
                //     {
                //         new Contact
                //         {
                //             ContactType = "phone",
                //             Value = fields[PHONE]
                //         }
                //     },
                //     Addresses = new List<Address>
                //     {
                //         new Address
                //         {
                //             Street = fields[ADDRESS],
                //             City = fields[CITY],
                //             Region = fields[STATE],
                //             PostCode = fields[ZIP],
                //             Country = "USA"
                //         }
                //     }
                // });
            }
        }


        await Applications.InsertManyAsync(seedData);

        //foreach (var item in seedData)
        //{
        //    // <database>.<collection>.<key>
        //    cache.Set($"bookstore.{nameof(Category)}.{item.Name}", item);
        //}
    }

    
}
