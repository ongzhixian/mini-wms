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

        var x = await responseMessage.Content.ReadAsStringAsync();

        //PagedData<UserRecord> a = new PagedData<UserRecord>()
        //{
        //    Page = 1,
        //    PageSize = 12,
        //    TotalRecordCount = 0,
        //    Data = new List<UserRecord>()
        //    {
        //        new UserRecord()
        //        {
        //            FirstName = "someFirstName",
        //            LastName = "someLastName",
        //            Username = "someUsername"
        //        }
        //    }
        //};

        //string j = "{\"TotalRecordCount\":11,\"Data\":[{\"Username\":\"apple\",\"FirstName\":\"apple\",\"LastName\":\"comp\"},{\"Username\":\"apple2\",\"FirstName\":\"apple\",\"LastName\":\"comp\"},{\"Username\":\"apple3\",\"FirstName\":\"asd\",\"LastName\":\"asd\"},{\"Username\":\"apple4\",\"FirstName\":\"asd\",\"LastName\":\"asd\"},{\"Username\":\"apple5\",\"FirstName\":\"asd\",\"LastName\":\"asd\"}],\"Page\":1,\"PageSize\":5}";
        //var resx1 = System.Text.Json.JsonSerializer.Deserialize<PagedData<UserRecord>>(j);

        //System.Text.Json.JsonSerializerOptions opt = new System.Text.Json.JsonSerializerOptions();
        //opt.PropertyNameCaseInsensitive = true;
        //opt.DictionaryKeyPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;

        //var res1 = System.Text.Json.JsonSerializer.Deserialize<PagedData<UserRecord>>(s);
        

        try
        {
            //var r = await responseMessage.Content.ReadFromJsonAsync<List<UserRecord>>(opt);
            //var r = await responseMessage.Content.ReadFromJsonAsync<PagedData<UserRecord>>()
            return await responseMessage.Content.ReadFromJsonAsync<PagedData<UserRecord>>();
        }
        catch (Exception)
        {
            throw;
        }

    }
}