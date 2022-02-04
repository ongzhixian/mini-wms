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

public class AuthenticationEndpoint : BearerHttpClient
{
    private readonly ILogger<AuthenticationEndpoint> logger;

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

    public AuthenticationEndpoint(
        ILogger<AuthenticationEndpoint> logger
        , JwtTokenService jwtTokenService
        , IHttpClientFactory httpClientFactory
        , IOptionsMonitor<HttpClientSetting> optionsMonitor
        , IHttpContextAccessor httpContextAccessor
        , PkedService pkedService

        )
        : base(HttpClientName.AuthenticationEndpoint, httpClientFactory, optionsMonitor, httpContextAccessor)
    {
        this.logger = logger;
        this.jwtTokenService = jwtTokenService;
        this.pkedService = pkedService;
    }

    public async Task<LoginResponse> PostAsync(string username, string password)
    {
        //string jsonStringData = JsonSerializer.Serialize(new LoginRequest(username, password, new SecurityCredential
        //{
        //    SecurityAlgorithm = SecurityAlgorithms.RsaOAEP
        //    , SecurityDigest = SecurityAlgorithms.Aes256CbcHmacSha512
        //    , Xml = await jwtTokenService.EncryptingKeyXmlAsync()
        //}));

        //var enc = await pkedService.EncryptAsync(
        //    jsonStringData,
        //    RsaKeyName.RecepEncryptingKey);


        //EncryptedMessage encryptedMessage = await pkedService.EncryptAsync<LoginRequest>(
        //    new LoginRequest(username, password, new SecurityCredential
        //    {
        //        SecurityAlgorithm = SecurityAlgorithms.RsaOAEP
        //        , SecurityDigest = SecurityAlgorithms.Aes256CbcHmacSha512
        //        , Xml = await jwtTokenService.EncryptingKeyXmlAsync()
        //    }), 
        //    RsaKeyName.RecepEncryptingKey);

        EncryptedMessage encryptedMessage = await pkedService.EncryptAsync<LoginRequest>(
            new LoginRequest(username, password), RsaKeyName.RecepEncryptingKey);


        var responseMessage = await httpClient.PostAsync("/api/authentication", 
            JsonContent.Create(encryptedMessage, mediaType: new MediaTypeHeaderValue(MediaTypeNames.Application.Json)));

        //var responseMessage = await httpClient.PostAsync("/api/authentication",
        //    JsonContent.Create(
        //        new LoginRequest(username, password, new SecurityCredential
        //        {
        //            SecurityAlgorithm = SecurityAlgorithms.RsaOAEP
        //            , SecurityDigest = SecurityAlgorithms.Aes256CbcHmacSha512
        //            , Xml = await jwtTokenService.EncryptingKeyXmlAsync()
        //        })
        //        , mediaType: new MediaTypeHeaderValue(MediaTypeNames.Application.Json)));

        responseMessage.EnsureSuccessStatusCode();

        LoginResponse response = await responseMessage.Content.ReadFromJsonAsync<LoginResponse>();

        return response;
    }
}