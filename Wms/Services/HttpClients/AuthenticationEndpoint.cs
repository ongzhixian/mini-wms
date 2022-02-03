using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Mini.Common.Models;
using Mini.Common.Requests;
using Mini.Common.Responses;
using System.Net.Http.Headers;
using System.Net.Mime;
using Wms.Models;

namespace Wms.Services.HttpClients;

public class AuthenticationEndpoint : BearerHttpClient
{
    private readonly ILogger<AuthenticationEndpoint> logger;

    private readonly JwtTokenService jwtTokenService;

    //private static class LogMessage
    //{
    //    // Conso.Services.AuthenticationHttpClient:JWT retrieved
    //    internal readonly static Action<ILogger, LoginResponse?, Exception?> RetrievedContent = LoggerMessage.Define<LoginResponse?>(
    //        LogLevel.Information, new EventId(292512, "JWT retrieved"), "JWT {jwt}");

    //    // Conso.Services.AuthenticationHttpClient:Retrieve null LoginResponse
    //    internal readonly static Action<ILogger, LoginResponse?, Exception?> RetrievedNullLoginResponse = LoggerMessage.Define<LoginResponse?>(
    //        LogLevel.Information, new EventId(301829, "Retrieve null LoginResponse"), "LoginResponse {LoginResponse}");
    //}

    public AuthenticationEndpoint(
        ILogger<AuthenticationEndpoint> logger
        , JwtTokenService jwtTokenService
        , IHttpClientFactory httpClientFactory
        , IOptionsMonitor<HttpClientSetting> optionsMonitor
        , IHttpContextAccessor httpContextAccessor
        )
        : base(HttpClientName.AuthenticationEndpoint, httpClientFactory, optionsMonitor, httpContextAccessor)
    {
        this.logger = logger;
        this.jwtTokenService = jwtTokenService;
    }

    public async Task<LoginResponse> PostAsync(string username, string password)
    {
        var responseMessage = await httpClient.PostAsync("/api/authentication",
            JsonContent.Create(
                new LoginRequest(username, password, new SecurityCredential
                {
                    SecurityAlgorithm = SecurityAlgorithms.RsaOAEP
                    , SecurityDigest = SecurityAlgorithms.Aes256CbcHmacSha512
                    , Xml = jwtTokenService.EncryptingKeyXml
                })
                , mediaType: new MediaTypeHeaderValue(MediaTypeNames.Application.Json)));

        responseMessage.EnsureSuccessStatusCode();

        LoginResponse response = await responseMessage.Content.ReadFromJsonAsync<LoginResponse>();

        return response;
    }
}