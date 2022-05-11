using Wms.Models.Shared;
using Wms.Services.HttpClients;

namespace Wms.Services;

public interface IUserProfileService
{
    Task<UserProfile> GetUserProfileAsync(string? username);
}

public class UserProfileService : IUserProfileService
{
    private readonly ILogger<UserProfileService> logger;

    private readonly AuthenticationEndpoint authenticationEndpoint;
    private readonly UserEndpoint userEndpoint;

    public UserProfileService(
        ILogger<UserProfileService> logger
        , AuthenticationEndpoint authenticationEndpoint
        , UserEndpoint userEndpoint)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.authenticationEndpoint = authenticationEndpoint ?? throw new ArgumentNullException(nameof(authenticationEndpoint));
        this.userEndpoint = userEndpoint ?? throw new ArgumentNullException(nameof(userEndpoint));
    }

    public async Task<UserProfile> GetUserProfileAsync(string? username)
    {
        return await Task.FromResult(new UserProfile());
    }
}
