using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;
using Wms.Models.Bookstore;

namespace Wms.Extensions;

public static class MongoCollectionBookExtensions
{
    public static async Task SetupIndexesAsync(this IMongoCollection<Book> books)
    {
        var documentCursor = await books.Indexes.ListAsync();

        if (documentCursor != null)
        {
            var indexes = await documentCursor.ToListAsync();

            if (indexes.Count <= 1)
            {
                var indexKeys = Builders<Book>.IndexKeys
                    .Ascending(m => m.Id);

                var indexOptions = new CreateIndexOptions { Unique = true };

                await books.Indexes.CreateOneAsync(new CreateIndexModel<Book>(indexKeys, indexOptions));
            }
        }
    }

    public static async Task SeedAsync(this IMongoCollection<Book> books, IMemoryCache cache)
    {
        if (await books.CountDocumentsAsync(_ => true) > 0)
        {
            return;
        }

        List<Book> seedData = new List<Book>();

        using (StreamReader sr = new("./Data/pubs/titles.csv"))
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
                const int TITLE_ID = 0;
                const int TITLE = 1;
                const int TYPE = 2;
                //const int PUB_ID = 3;
                //const int PRICE = 4;
                //const int ADVANCE = 5;
                //const int ROYALTY = 6;
                //const int YTD_SALES = 7;
                //const int NOTES = 8;
                //const int PUBDATE = 9;

                seedData.Add(new Book
                {
                    Id = fields[TITLE_ID],
                    Title = fields[TITLE],
                    Categories = new List<Category>
                    { 
                        new Category
                        {
                            Name = fields[TYPE].Replace("\"", string.Empty).Trim()
                        }
                    }
                });
            }
        }

        await books.InsertManyAsync(seedData);

        //foreach (var item in seedData)
        //{
        //    // <database>.<collection>.<key>
        //    cache.Set($"bookstore.{nameof(Category)}.{item.Name}", item);
        //}
    }

    
}
