using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;
using Wms.Extensions;
using Wms.Models.Bookstore;

namespace Wms.DbContexts;

public class BookstoreContext
{
    public readonly IMongoCollection<Author> Authors;
    public readonly IMongoCollection<Category> Categories;
    public readonly IMongoCollection<Book> Books;
    public readonly IMongoCollection<Country> Countries;
    public readonly IMongoCollection<ContactType> ContactTypes;

    public string DatabaseName { get; private set; }

    private readonly IMongoClient mongoClient;
    private readonly IMongoDatabase db;
    private IMemoryCache cache;

    public BookstoreContext(string connectionString, IMemoryCache cache)
    {
        mongoClient = new MongoClient(connectionString);
        DatabaseName = MongoUrl.Create(connectionString).DatabaseName;
        db = mongoClient.GetDatabase(DatabaseName);

        this.cache = cache;

        Authors = db.GetCollection<Author>(nameof(Author).ToCamelCase());
        Categories = db.GetCollection<Category>(nameof(Category).ToCamelCase());
        Books = db.GetCollection<Book>(nameof(Book).ToCamelCase());
        Countries = db.GetCollection<Country>(nameof(Country).ToCamelCase());
        ContactTypes = db.GetCollection<ContactType>(nameof(ContactType).ToCamelCase());

        //this.Users = db.GetCollection<User>("Users");
        // services.AddSingleton<IMongoCollection<User>>(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<User>("user"));
        // services.AddSingleton<IMongoCollection<Bookmark>>(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<Bookmark>("bookmark"));
        // services.AddSingleton<IMongoCollection<Note>>(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<Note>("note"));
    }

    public async Task SetupIndexesAsync()
    {
        await Categories.SetupIndexesAsync();

        await Countries.SetupIndexesAsync();

        await ContactTypes.SetupIndexesAsync();

        await Authors.SetupIndexesAsync();

        await Books.SetupIndexesAsync();
    }

    public async Task InitializeBookstoreAsync()
    {
        await Categories.SeedAsync(cache);

        await Countries.SeedAsync(cache);
        
        await ContactTypes.SeedAsync(cache);

        // Data

        await Authors.SeedAsync(cache);

        await Books.SeedAsync(cache);

        //await SeedCategoriesAsync();
        // await SeedAuthorsAsync();
        //await SeedBooksAsync();
    }
    
    // METHODS

    private async Task<bool> CategoryExistsAsync(string category)
    {
        var filter = Builders<Category>.Filter.Eq(t => t.Name, category);

        return await Categories.CountDocumentsAsync(filter) > 0;
    }

    private async Task InsertCategoryIfNotExistsAsync(string category)
    {
        if (!await CategoryExistsAsync(category))
        {
            await Categories.InsertOneAsync(new Category { Name = category });
        }

        // Code for Upsert (for reference)
        //var filter = Builders<Category>.Filter.Eq(t => t.Name, category);
        //var updateDefinition = Builders<Category>.Update
        //        .Set(m => m.Name, category);
        //var updateResult = await Categories.UpdateOneAsync(filter, updateDefinition, new UpdateOptions { IsUpsert = true });

        // We are not using upsert because we are following the principle of not touching the data more than we have to.
        // Since what we want to do is add if it does not exists in database;
        // Let's not upsert just for the sake of convenience.
    }

    private async Task<Category?> GetCategoryIfExistsAsync(string category)
    {
        string cacheKey = $"bookstore-{nameof(Category)}-{category}";

        if (cache.TryGetValue(cacheKey, out Category cachedValue))
        {
            return cachedValue;
        }

        if (!await CategoryExistsAsync(category))
        {
            return default;
        }

        var filter = Builders<Category>.Filter.Eq(t => t.Name, category);

        var find = await Categories.FindAsync(filter);

        Category? result = await find.FirstOrDefaultAsync();

        if (result != null)
        {
            cache.Set(cacheKey, result, new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromHours(1)
            });
        }

        return result;
    }

    // GetCategoryIdIfExistsElseAdd
    // AddOrGetExistingCategory

    private async Task zzzGetCategoryAsync(string category)
    {
        // Get Category Id
        var filter = Builders<Category>.Filter.Eq(t => t.Name, category);
        var count = await Categories.CountDocumentsAsync(filter);

        if (count > 1)
        {
            throw new Exception("Too many records matching criteria found");
        }

        if (count == 0)
        {
            throw new Exception("No records matching criteria found");
        }

        var find = await Categories.FindAsync(filter);
        Category? res = await find.FirstOrDefaultAsync();

        //    var asyncCursor = await Categories.FindAsync(filter);
        //var list = await asyncCursor.ToListAsync();

        //var count = await Categories.CountDocumentsAsync(filter);

        //Task<Category>? www = await x.FirstOrDefaultAsync();
    }

    // SEED




    //private async Task SeedCategoriesAsync()
    //{
    //    if (await Categories.CountDocumentsAsync(_ => true) > 0)
    //    {
    //        return;
    //    }

    //    var categories = new List<Category>
    //    {
    //        new Category { Name = "business" },
    //        new Category { Name = "psychology" },
    //        new Category { Name = "traditional_cooking" },
    //        new Category { Name = "modern-cooking" },
    //        new Category { Name = "computing" },
    //    };

    //    await Categories.InsertManyAsync(categories);

    //    //Categories.SeedAsync(cache);
    //}

    // SETUP INDEXES



    //private async Task SetupIndexesForBookAsync()
    //{
    //    try
    //    {
    //        var documentCursor = await Books.Indexes.ListAsync();

    //        if (documentCursor != null)
    //        {
    //            var indexes = await documentCursor.ToListAsync();

    //            if (indexes.Count <= 1)
    //            {
    //                var indexKeys = Builders<Book>.IndexKeys
    //                    .Ascending(m => m.Title)
    //                    .Ascending(m => m.Author);

    //                var indexOptions = new CreateIndexOptions { Unique = true };

    //                await Books.Indexes.CreateOneAsync(new CreateIndexModel<Book>(indexKeys, indexOptions));
    //            }
    //        }
    //    }
    //    catch (TimeoutException)
    //    {
    //        Console.WriteLine("Timeout on SetupIndexesForBookAsync; MongoDb maybe unavailable.");
    //    }
    //}

    //private async Task SetupIndexesForAuthorAsync()
    //{
    //    try
    //    {
    //        var documentCursor = await Authors.Indexes.ListAsync();

    //        if (documentCursor != null)
    //        {
    //            var indexes = await documentCursor.ToListAsync();

    //            if (indexes.Count <= 1)
    //            {
    //                var indexKeys = Builders<Author>.IndexKeys
    //                    .Ascending(m => m.)
    //                    .Ascending(m => m.Author);

    //                var indexOptions = new CreateIndexOptions { Unique = true };

    //                await Books.Indexes.CreateOneAsync(new CreateIndexModel<Book>(indexKeys, indexOptions));
    //            }
    //        }
    //    }
    //    catch (TimeoutException)
    //    {
    //        Console.WriteLine("Timeout on SetupIndexesForBookAsync; MongoDb maybe unavailable.");
    //    }
    //}
}