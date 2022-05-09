using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;
using Wms.Extensions;
using Wms.Models.Shared;

namespace Wms.DbContexts;

public class SharedMongoDbContext
{
    public readonly IMongoCollection<Models.Shared.Application> Applications;

    public readonly IMongoCollection<Models.Shared.Role> Roles;

    public readonly IMongoCollection<Models.Shared.User> Users;

    public readonly IMongoCollection<Models.Shared.UserProfile> UserProfiles;

    public string DatabaseName { get; private set; }

    private readonly IMongoClient mongoClient;
    private readonly IMongoDatabase db;
    private IMemoryCache cache;

    public SharedMongoDbContext(string connectionString, IMemoryCache cache)
    {
        mongoClient = new MongoClient(connectionString);
        // DatabaseName = MongoUrl.Create(connectionString).DatabaseName;
        DatabaseName = "shared";
        db = mongoClient.GetDatabase(DatabaseName);

        this.cache = cache;

        Applications = db.GetCollection<Application>(nameof(Application).ToCamelCase());

        Roles = db.GetCollection<Role>(nameof(Role).ToCamelCase());

        Users = db.GetCollection<User>(nameof(User).ToCamelCase());

        //UserProfiles = db.GetCollection<UserProfile>(nameof(UserProfile).ToCamelCase());

        //this.Users = db.GetCollection<User>("Users");
        // services.AddSingleton<IMongoCollection<User>>(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<User>("user"));
        // services.AddSingleton<IMongoCollection<Bookmark>>(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<Bookmark>("bookmark"));
        // services.AddSingleton<IMongoCollection<Note>>(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<Note>("note"));
    }

    public async Task SetupIndexesAsync()
    {
        await Applications.SetupIndexesAsync();

        await Roles.SetupIndexesAsync();

        await Users.SetupIndexesAsync();
    }

    public async Task SeedDataAsync()
    {
        await Applications.SeedAsync(cache);

        await Roles.SeedAsync(cache);

        await Users.SeedAsync(cache);
    }

}