using MongoDB.Driver;
using Wms.Models.Data.Bookstore;
using Wms.Extensions;

namespace Wms.DbContexts;

public class BookstoreContext
{
    public readonly IMongoCollection<Book> Books;

    public readonly IMongoCollection<Category> Categories;

    public string DatabaseName { get; private set; }

    private readonly IMongoClient mongoClient;
    private readonly IMongoDatabase db;

    public BookstoreContext(string connectionString)
    {
        mongoClient = new MongoClient(connectionString);
        DatabaseName = MongoUrl.Create(connectionString).DatabaseName;
        db = mongoClient.GetDatabase(DatabaseName);

        Books = db.GetCollection<Book>(nameof(Book).ToCamelCase());
        Categories = db.GetCollection<Category>(nameof(Category).ToCamelCase());

        //this.Users = db.GetCollection<User>("Users");
        // services.AddSingleton<IMongoCollection<User>>(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<User>("user"));
        // services.AddSingleton<IMongoCollection<Bookmark>>(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<Bookmark>("bookmark"));
        // services.AddSingleton<IMongoCollection<Note>>(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<Note>("note"));
    }

    public async Task SetupIndexesAsync()
    {
        await SetupIndexesForBookAsync();
        await SetupIndexesForCategoryAsync();
    }


    public void InitializeBookstore()
    {

    }

    private async Task SetupIndexesForCategoryAsync()
    {
        var documentCursor = await Categories.Indexes.ListAsync();

        if (documentCursor != null)
        {
            var indexes = await documentCursor.ToListAsync();

            if (indexes.Count <= 1)
            {
                var indexKeys = Builders<Category>.IndexKeys
                    .Ascending(m => m.Name);

                var indexOptions = new CreateIndexOptions { Unique = true };

                await Categories.Indexes.CreateOneAsync(new CreateIndexModel<Category>(indexKeys, indexOptions));
            }
        }
    }

    private async Task SetupIndexesForBookAsync()
    {
        var documentCursor = await Books.Indexes.ListAsync();

        if (documentCursor != null)
        {
            var indexes = await documentCursor.ToListAsync();

            if (indexes.Count <= 1)
            {
                var indexKeys = Builders<Book>.IndexKeys
                    .Ascending(m => m.Title)
                    .Ascending(m => m.Author);

                var indexOptions = new CreateIndexOptions { Unique = true };

                await Books.Indexes.CreateOneAsync(new CreateIndexModel<Book>(indexKeys, indexOptions));
            }
        }
    }
}