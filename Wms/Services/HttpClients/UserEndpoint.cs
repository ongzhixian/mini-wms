using Microsoft.Extensions.Options;
using Mini.Wms.DomainMessages;
using System.Net.Http.Headers;
using System.Net.Mime;
using Wms.Models;


namespace Wms.Services.HttpClients;

public class UserEndpoint : BearerHttpClient
{
    private readonly ILogger<UserEndpoint> logger;

    public UserEndpoint(
        ILogger<UserEndpoint> logger
        , IHttpClientFactory httpClientFactory
        , IOptionsMonitor<HttpClientSetting> optionsMonitor
        , IHttpContextAccessor httpContextAccessor
        )
        : base(HttpClientName.UserEndpoint, httpClientFactory, optionsMonitor, httpContextAccessor)
    {
        this.logger = logger;
    }

    internal async Task<string> GetUserAsync()
    {
        var responseMessage = await httpClient.GetAsync("/api/user");

        responseMessage.EnsureSuccessStatusCode();

        var response = await responseMessage.Content.ReadAsStringAsync();

        return response;
    }

    internal async Task<string> AddUserAsync(NewUserViewModel newUser)
    {
        //Mini.Wms.DomainMessages.NewUser message = new Mini.Wms.DomainMessages.NewUser(newUser.Username, newUser.FirstName, newUser.LastName);
        //{
        //    FirstName = newUser.FirstName,
        //    LastName = newUser.LastName,
        //    Username = newUser.Username
        //};

        //Mini.Wms.DomainMessages.AddUser message2 = newUser with
        //{

        //};

        //NewUserDomainModel model = newUser with
        //{
        //};
        NewUserMessage message = new NewUserMessage(newUser.Username, newUser.FirstName, newUser.LastName);
        //{
        //    Username = newUser.Username,
        //    FirstName = newUser.FirstName,
        //    LastName = newUser.LastName
        //};

        var responseMessage = await httpClient.PostAsync("/api/user",
            JsonContent.Create(message, mediaType: new MediaTypeHeaderValue(MediaTypeNames.Application.Json)));

        responseMessage.EnsureSuccessStatusCode();

        // LoginResponse response = await responseMessage.Content.ReadFromJsonAsync<LoginResponse>()

        var response = await responseMessage.Content.ReadAsStringAsync();

        return response;
    }

    public async Task<IEnumerable<UserRecord>> GetAllUsers()
    {
        var responseMessage = await httpClient.GetAsync("/api/user");

        responseMessage.EnsureSuccessStatusCode();

        var response = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<UserRecord>>();

        if (response != null)
            return response;

        return new List<UserRecord>();
    }
}