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

[Obsolete]
public class obsAuthenticationHttpClient
{
    private readonly ILogger<obsAuthenticationHttpClient> logger;

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

    public obsAuthenticationHttpClient(
        ILogger<obsAuthenticationHttpClient> logger
        , IHttpClientFactory httpClientFactory
        , HttpClient httpClient
        , IOptionsMonitor<HttpClientSetting> optionsMonitor
        , JwtTokenService jwtTokenService
        , IHttpContextAccessor httpContextAccessor
        )
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

        this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

        var httpClientSetting = optionsMonitor.Get(HttpClientName.AuthenticationEndpoint);

        httpClientSetting.EnsureIsValid();

        httpClient.BaseAddress = new Uri(httpClientSetting.BaseAddress);

        this.jwtTokenService = jwtTokenService ?? throw new ArgumentNullException(nameof(logger));

        if ((httpContextAccessor.HttpContext != null) && httpContextAccessor.HttpContext.Session.Keys.Contains("JWT"))
        {
            string token = httpContextAccessor.HttpContext.Session.GetString("JWT") ?? throw new NullReferenceException("Session[JWT] is null");

            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }


    public async Task<LoginResponse> PostAuthenticationAsync(string username, string password)
    {
        try
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
        catch (Exception ex)
        {
            throw;
        }

    }

    //public async Task<JwtSecurityToken> GetJwtAsync(string username, string password)
    //{
    //    try
    //    {
    //        var responseMessage = await httpClient.PostAsync("/api/authentication", JsonContent.Create(
    //            new LoginRequest(username, password, new SecurityCredential
    //            {
    //                SecurityAlgorithm = SecurityAlgorithms.RsaOAEP
    //                ,
    //                SecurityDigest = SecurityAlgorithms.Aes256CbcHmacSha512
    //                ,
    //                Xml = jwtTokenService.EncryptingKeyXml
    //            })
    //            , mediaType: new MediaTypeHeaderValue(MediaTypeNames.Application.Json)));

    //        responseMessage.EnsureSuccessStatusCode();

    //        var response = await responseMessage.Content.ReadFromJsonAsync<LoginResponse>();

    //        //if (response)
    //        //{
    //        //    LogMessage.RetrievedNullLoginResponse(logger, response, null);
    //        //    return response;
    //        //}

    //        var token = jwtTokenService.ParseAsync(response.Jwt);

    //        LogMessage.RetrievedContent(logger, response, null);

    //        return token;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw;
    //    }

    //}

}
