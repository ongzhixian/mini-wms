using Microsoft.Extensions.Options;
using Mini.Common.Models;
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
        NewUserMessage message = new(newUser.Username, newUser.FirstName, newUser.LastName);

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

    internal async Task<PagedData<UserRecord>> GetAllUsers(PagedDataOptions pagedData)
    {
        var responseMessage = await httpClient.GetAsync("/api/user" + pagedData.ToQueryString());

        responseMessage.EnsureSuccessStatusCode();

        return await responseMessage.Content.ReadFromJsonAsync<PagedData<UserRecord>>();

    }
}