using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Mini.Common.Models;
using Mini.Common.Requests;
using Mini.Common.Responses;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Mime;
using Wms.Models;

namespace Wms.Services.HttpClients;

public class UserServiceHttpClient
{
    private readonly ILogger<UserServiceHttpClient> logger;
    
    private readonly HttpClient httpClient;

    private readonly IJwtTokenService jwtTokenService;

    private static class LogMessage
    {
        //// Conso.Services.AuthenticationHttpClient:JWT retrieved
        //internal readonly static Action<ILogger, LoginResponse?, Exception?> RetrievedContent = LoggerMessage.Define<LoginResponse?>(
        //    LogLevel.Information, new EventId(292512, "JWT retrieved"), "JWT {jwt}");

        //// Conso.Services.AuthenticationHttpClient:Retrieve null LoginResponse
        //internal readonly static Action<ILogger, LoginResponse?, Exception?> RetrievedNullLoginResponse = LoggerMessage.Define<LoginResponse?>(
        //    LogLevel.Information, new EventId(301829, "Retrieve null LoginResponse"), "LoginResponse {LoginResponse}");
    }

    public UserServiceHttpClient(
        ILogger<UserServiceHttpClient> logger
        , IHttpClientFactory httpClientFactory
        , IOptionsMonitor<HttpClientSetting> optionsMonitor
        , IJwtTokenService jwtTokenService
        )
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

        this.httpClient = httpClientFactory.CreateClient("");

        var c1 = httpClientFactory.CreateClient("");

        var c2 = httpClientFactory.CreateClient("asd");

        var c3 = httpClientFactory.CreateClient("authenticatedClient");

        //httpClient ?? throw new ArgumentNullException(nameof(httpClient));

        var httpClientSetting = optionsMonitor.Get(HttpClientName.UserEndpoint);
        
        httpClientSetting.EnsureIsValid();
        
        httpClient.BaseAddress = new Uri(httpClientSetting.BaseAddress);

        this.jwtTokenService = jwtTokenService ?? throw new ArgumentNullException(nameof(logger));
    }

    internal async Task AddUserAsync(NewUserViewModel newUser)
    {
        try
        {
            var responseMessage = await httpClient.PostAsync("/api/user", 
                JsonContent.Create(newUser, mediaType: new MediaTypeHeaderValue(MediaTypeNames.Application.Json)));

            responseMessage.EnsureSuccessStatusCode();

            var response = await responseMessage.Content.ReadAsStringAsync();

            //var response = await responseMessage.Content.ReadFromJsonAsync<LoginResponse>();

            //var token = jwtTokenService.Parse(response.Jwt);

            //LogMessage.RetrievedContent(logger, response, null);

            //return token;
        }
        catch (Exception)
        {

            throw;
        }
    }

}
