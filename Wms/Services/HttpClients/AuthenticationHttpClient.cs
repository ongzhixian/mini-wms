using Microsoft.Extensions.Options;
using Mini.Common.Requests;
using Mini.Common.Responses;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;
using Wms.Models;

namespace Wms.Services.HttpClients;

public class AuthenticationHttpClient
{
    private readonly ILogger<AuthenticationHttpClient> logger;
    
    private readonly HttpClient httpClient;

    private readonly JwtTokenService jwtTokenService;

    private static class LogMessage
    {
        // Conso.Services.AuthenticationHttpClient:JWT retrieved
        internal readonly static Action<ILogger, LoginResponse?, Exception?> RetrievedContent = LoggerMessage.Define<LoginResponse?>(
            LogLevel.Information, new EventId(292512, "JWT retrieved"), "JWT {jwt}");

        // Conso.Services.AuthenticationHttpClient:Retrieve null LoginResponse
        internal readonly static Action<ILogger, LoginResponse?, Exception?> RetrievedNullLoginResponse = LoggerMessage.Define<LoginResponse?>(
            LogLevel.Information, new EventId(301829, "Retrieve null LoginResponse"), "LoginResponse {LoginResponse}");
    }

    public AuthenticationHttpClient(
        ILogger<AuthenticationHttpClient> logger
        , HttpClient httpClient
        , IOptionsMonitor<HttpClientSetting> optionsMonitor
        , JwtTokenService jwtTokenService
        )
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

        this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

        var httpClientSetting = optionsMonitor.Get(HttpClientName.Authentication);
        httpClientSetting.EnsureIsValid();
        httpClient.BaseAddress = new Uri(httpClientSetting.BaseAddress);

        this.jwtTokenService = jwtTokenService ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<JwtSecurityToken> GetJwtAsync(string username, string password)
    {
        try
        {
            var responseMessage = await httpClient.PostAsync("/api/authentication", JsonContent.Create(
                new LoginRequest(username, password)
                , mediaType: new MediaTypeHeaderValue(MediaTypeNames.Application.Json)));

            responseMessage.EnsureSuccessStatusCode();

            var res = await responseMessage.Content.ReadAsStringAsync();

            var response = await responseMessage.Content.ReadFromJsonAsync<LoginResponse>();

            //if (response)
            //{
            //    LogMessage.RetrievedNullLoginResponse(logger, response, null);
            //    return response;
            //}

            var token = jwtTokenService.Parse(response.Jwt);

            LogMessage.RetrievedContent(logger, response, null);
            
            return token;
        }
        catch (Exception ex)
        {
            throw;
        }

    }

}
