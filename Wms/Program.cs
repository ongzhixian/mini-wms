using Mini.Common.Models;
using Mini.Wms.DomainMessages;
using System.Reflection;
using Wms;
using Wms.Models;
using Wms.Services;

var builder = WebApplication.CreateBuilder(args);

AppStartup.SetupAntiForgery(builder.Services);

AppStartup.SetupSession(builder.Services);

AppStartup.SetupAuthentication(builder.Services);

AppStartup.SetupAuthorization(builder.Services);

AppStartup.SetupHttpClient(builder.Configuration, builder.Services);

AppStartup.SetupServices(builder.Configuration, builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseSession();

app.MapRazorPages();

app.MapPost("/api/values", async (DataRequest dataRequest, IUserService userService) =>
{
    //db.Todos.Add(todo);
    //await db.SaveChangesAsync();
    //return Results.Created($"/todoitems/{todo.Id}", todo);
    PagedDataOptions PagedData = new PagedDataOptions();

    PagedData.Page = (uint)dataRequest.Page;
    PagedData.PageSize = (uint)dataRequest.PageSize;
    PagedData.DataType = dataRequest.DataType;
    PagedData.DataFieldList.Add(new DataField("Username", true, 1));
    PagedData.DataFieldList.Add(new DataField("FirstName", true, 2));
    PagedData.DataFieldList.Add(new DataField("LastName", true, 3));

    try
    {
        var result = await userService.GetAllUsersAsync(PagedData);

        var UserList = result.Data;

        var fieldNames = typeof(UserRecord).GetProperties(BindingFlags.Instance | BindingFlags.Public).Select(r => r.Name).ToList();

        return Results.Ok(new DataResponse<UserRecord>
        {
            FieldNames = fieldNames,
            Data = result.Data,
            TotalRecordCount = result.TotalRecordCount
        });
    }
    catch (Exception ex)
    {
        throw;
    }


});

//var dr = new DataRequest
//{
//    DataType = "User"
//};

//dr.SortFields.Add(new SortField { FieldName = "firstName", SortDirection = SortDirection.Ascending });
//dr.SortFields.Add(new SortField { FieldName = "lastName", SortDirection = SortDirection.Descending});

//var json = System.Text.Json.JsonSerializer.Serialize<DataRequest>(dr);
//Console.WriteLine(json);

//UserRecord rec = new UserRecord()
//{
//    FirstName = "someFirstName",
//    LastName = "someLastName",
//    Username = "someUsername"
//};

//var json = System.Text.Json.JsonSerializer.Serialize(rec);

//Console.WriteLine(json);

//var rec1 = System.Text.Json.JsonSerializer.Deserialize<UserRecord>(json);

//Console.WriteLine(rec1.FirstName);

//string j = "{\"TotalRecordCount\":11,\"Data\":[{\"Username\":\"apple\",\"FirstName\":\"apple\",\"LastName\":\"comp\"},{\"Username\":\"apple2\",\"FirstName\":\"apple\",\"LastName\":\"comp\"},{\"Username\":\"apple3\",\"FirstName\":\"asd\",\"LastName\":\"asd\"},{\"Username\":\"apple4\",\"FirstName\":\"asd\",\"LastName\":\"asd\"},{\"Username\":\"apple5\",\"FirstName\":\"asd\",\"LastName\":\"asd\"}],\"Page\":1,\"PageSize\":5}";

//var resx1 = System.Text.Json.JsonSerializer.Deserialize<PagedData<UserRecord>>(j);

//string t = "{\"totalRecordCount\":11,\"data\":[{\"username\":\"apple\",\"firstName\":\"apple\",\"lastName\":\"comp\"},{\"username\":\"apple2\",\"firstName\":\"apple\",\"lastName\":\"comp\"},{\"username\":\"apple3\",\"firstName\":\"asd\",\"lastName\":\"asd\"},{\"username\":\"apple4\",\"firstName\":\"asd\",\"lastName\":\"asd\"},{\"username\":\"apple5\",\"firstName\":\"asd\",\"lastName\":\"asd\"}],\"page\":1,\"pageSize\":5}";
string t = "{\"TotalRecordCount\":11,\"data\":[{\"Username\":\"apple\",\"FirstName\":\"apple\",\"LastName\":\"comp\"}],\"Page\":1,\"PageSize\":5}";
var res1 = System.Text.Json.JsonSerializer.Deserialize<PagedData<UserRecord>>(t);


app.Run();
