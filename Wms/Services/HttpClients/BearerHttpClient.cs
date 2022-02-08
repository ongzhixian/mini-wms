using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using Wms.Models;

namespace Wms.Services.HttpClients;

public abstract class BearerHttpClient
{
    protected readonly HttpClient httpClient;

    protected BearerHttpClient(string httpClientSettingName
        , IHttpClientFactory httpClientFactory
        , IOptionsMonitor<HttpClientSetting> optionsMonitor
        , IHttpContextAccessor httpContextAccessor)
    {
        var httpClientSetting = optionsMonitor.Get(httpClientSettingName); // HttpClientName.Authentication

        httpClientSetting.EnsureIsValid();

        httpClient = httpClientFactory.CreateClient(HttpClientName.BearerHttpClient);

        httpClient.BaseAddress = new Uri(httpClientSetting.BaseAddress);

        if ((httpContextAccessor.HttpContext != null) && httpContextAccessor.HttpContext.Session.Keys.Contains(SessionKeyName.JWT))
        {
            string token = httpContextAccessor.HttpContext.Session.GetString(SessionKeyName.JWT) ?? throw new NullReferenceException($"Session[{SessionKeyName.JWT}] is null");

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        else
        {
            // How to request for a new Jwt?
            
        }
    }
}
