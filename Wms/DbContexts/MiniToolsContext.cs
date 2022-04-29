using MongoDB.Driver;

namespace Wms.DbContexts;

public class MiniToolsContext
{
    //IMongoCollection<User> Users;

    public MiniToolsContext(string connectionString)
    {
        //string miniToolsConnectionString = configuration.GetValue<string>("mongodb:minitools:ConnectionString");
        IMongoClient mongoClient = new MongoClient(connectionString);
        string databaseName = MongoUrl.Create(connectionString).DatabaseName;
        var db = mongoClient.GetDatabase(databaseName);

        //this.Users = db.GetCollection<User>("Users");

        // services.AddSingleton<IMongoCollection<User>>(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<User>("user"));
        // services.AddSingleton<IMongoCollection<Bookmark>>(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<Bookmark>("bookmark"));
        // services.AddSingleton<IMongoCollection<Note>>(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<Note>("note"));
    }

}