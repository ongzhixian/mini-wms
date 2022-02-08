using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Mini.Common.Models;
using Mini.Common.Requests;
using Mini.Common.Responses;
using Mini.Common.Services;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;
using Wms.Models;

namespace Wms.Services.HttpClients;

public class RefreshTokenEndpoint : BearerHttpClient
{
    private readonly ILogger<RefreshTokenEndpoint> logger;

    private readonly JwtTokenService jwtTokenService;

    private readonly PkedService pkedService;

    //private static class LogMessage
    //{
    //    // Conso.Services.AuthenticationHttpClient:JWT retrieved
    //    internal readonly static Action<ILogger, LoginResponse?, Exception?> RetrievedContent = LoggerMessage.Define<LoginResponse?>(
    //        LogLevel.Information, new EventId(292512, "JWT retrieved"), "JWT {jwt}");

    //    // Conso.Services.AuthenticationHttpClient:Retrieve null LoginResponse
    //    internal readonly static Action<ILogger, LoginResponse?, Exception?> RetrievedNullLoginResponse = LoggerMessage.Define<LoginResponse?>(
    //        LogLevel.Information, new EventId(301829, "Retrieve null LoginResponse"), "LoginResponse {LoginResponse}");
    //}

    public RefreshTokenEndpoint(
        ILogger<RefreshTokenEndpoint> logger
        , JwtTokenService jwtTokenService
        , IHttpClientFactory httpClientFactory
        , IOptionsMonitor<HttpClientSetting> optionsMonitor
        , IHttpContextAccessor httpContextAccessor
        , PkedService pkedService
        )
        : base(HttpClientName.RefreshTokenEndpoint, httpClientFactory, optionsMonitor, httpContextAccessor)
    {
        this.logger = logger;
        this.jwtTokenService = jwtTokenService;
        this.pkedService = pkedService;
    }

    public async Task<LoginResponse> PostAsync(string username, string password)
    {
        EncryptedMessage encryptedMessage = await pkedService.EncryptAsync<LoginRequest>(
            new LoginRequest(username, password, new SecurityCredential
            {
                SecurityAlgorithm = SecurityAlgorithms.RsaOAEP
                , SecurityDigest = SecurityAlgorithms.Aes256CbcHmacSha512
                , Xml = await jwtTokenService.EncryptingKeyXmlAsync()
            }), RsaKeyName.RecepEncryptingKey);

        var responseMessage2 = await httpClient.PostAsync("/api/refreshToken",
            JsonContent.Create(encryptedMessage, mediaType: new MediaTypeHeaderValue(MediaTypeNames.Application.Json)));

        responseMessage2.EnsureSuccessStatusCode();

        //LoginResponse response = await responseMessage.Content.ReadFromJsonAsync<LoginResponse>();
        LoginResponse response = await responseMessage2.Content.ReadFromJsonAsync<LoginResponse>();

        return response;
    }
}