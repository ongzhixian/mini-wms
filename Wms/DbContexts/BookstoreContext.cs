using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Wms.Models.Data.Bookstore;

namespace Wms.DbContexts;

public class BookstoreContext
{
    readonly IMongoCollection<Book> Books;
    public string DatabaseName { get; private set; }

    private readonly IMongoClient mongoClient;
    private readonly IMongoDatabase db;

    public BookstoreContext(string connectionString)
    {
        mongoClient = new MongoClient(connectionString);
        DatabaseName = MongoUrl.Create(connectionString).DatabaseName;
        db = mongoClient.GetDatabase(DatabaseName);

        Books = db.GetCollection<Book>("Books");
        //this.Users = db.GetCollection<User>("Users");
        // services.AddSingleton<IMongoCollection<User>>(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<User>("user"));
        // services.AddSingleton<IMongoCollection<Bookmark>>(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<Bookmark>("bookmark"));
        // services.AddSingleton<IMongoCollection<Note>>(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<Note>("note"));
    }

}