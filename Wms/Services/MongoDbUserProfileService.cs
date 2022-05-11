using Mini.Common.Models;
using Mini.Common.Responses;
using Mini.Wms.DomainMessages;
using Wms.DbContexts;
using Wms.Extensions;
using Wms.Models;
using Wms.Models.Shared;
using Wms.Services.HttpClients;

namespace Wms.Services;

public class MongoDbUserProfileService : IUserProfileService
{
    private readonly ILogger<UserProfileService> logger;

    private readonly SharedMongoDbContext sharedMongoDbContext;

    public MongoDbUserProfileService(
        ILogger<UserProfileService> logger
        , SharedMongoDbContext sharedMongoDbContext
        )
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.sharedMongoDbContext = sharedMongoDbContext ?? throw new ArgumentNullException(nameof(sharedMongoDbContext));
    }

    public async Task<UserProfile> GetUserProfileAsync(string? username)
    {
        if (username == null)
        {
            return new UserProfile();
        }

        var user = await sharedMongoDbContext.Users.FindFindFirstOrDefaultAsync(username);

        if (user == null)
        {
            return new UserProfile();
        }

        return new UserProfile
        {
            PreferredApplication = user.PreferredApplication
        };
    }
}
