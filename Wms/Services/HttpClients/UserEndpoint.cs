using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Mime;
using Wms.Models;


namespace Wms.Services.HttpClients;

public class UserEndpoint : BearerHttpClient
{
    private readonly ILogger<UserEndpoint> logger;

    private readonly JwtTokenService jwtTokenService;

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

    internal async Task<string> AddUserAsync(NewUserViewModel newUser)
    {
        var responseMessage = await httpClient.PostAsync("/api/user",
            JsonContent.Create(newUser, mediaType: new MediaTypeHeaderValue(MediaTypeNames.Application.Json)));

        responseMessage.EnsureSuccessStatusCode();

        //LoginResponse response = await responseMessage.Content.ReadFromJsonAsync<LoginResponse>();

        var response = await responseMessage.Content.ReadAsStringAsync();

        return response;
    }
}