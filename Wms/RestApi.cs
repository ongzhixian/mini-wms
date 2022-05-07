using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.Json;
using Wms.DbContexts;
using Wms.Models;
using Wms.Models.Bookstore;

namespace Wms;

internal static class RestApi
{
    internal static void Setup(WebApplication app)
    {
        SetupCountrySearch(app);
    }

    private static void SetupCountrySearch(WebApplication app)
    {
        app.MapGet("/api/search/country", async (string? q, BookstoreContext bookstoreContext) =>
        {
            Console.WriteLine(q);

            if (q == null)
            {
                Console.WriteLine("q is null");
            }

            //var rg = new MongoDB.Bson.BsonRegularExpression()
            var rg = new BsonRegularExpression($"/.*{q}.*/i");

            FilterDefinition<Country> filter = Builders<Country>.Filter.Regex(m => m.Name, rg);

            var options = new FindOptions<Country>();
            //options.Skip
            options.Limit = 10;
            options.Sort = Builders<Country>.Sort.Ascending(m => m.Name);

            IAsyncCursor<Country> docCursor = await bookstoreContext.Countries.FindAsync(filter, options);

            var result = await docCursor.ToListAsync();

            
            return Results.Ok(
                JsonSerializer.Serialize(result.Select(m => m.Name))
                );

            //db.Todos.Add(todo);
            //await db.SaveChangesAsync();
            //return Results.Created($"/todoitems/{todo.Id}", todo);
            //PagedDataOptions PagedData = new PagedDataOptions();

            //PagedData.Page = (uint)dataRequest.Page;
            //PagedData.PageSize = (uint)dataRequest.PageSize;
            //PagedData.DataType = dataRequest.DataType;
            //PagedData.DataFieldList.Add(new DataField("Username", true, 1));
            //PagedData.DataFieldList.Add(new DataField("FirstName", true, 2));
            //PagedData.DataFieldList.Add(new DataField("LastName", true, 3));

            //try
            //{
            //    var result = await userService.GetAllUsersAsync(PagedData);

            //    var UserList = result.Data;

            //    var fieldNames = typeof(UserRecord).GetProperties(BindingFlags.Instance | BindingFlags.Public).Select(r => r.Name).ToList();

            //    return Results.Ok(new DataResponse<UserRecord>
            //    {
            //        FieldNames = fieldNames,
            //        Data = result.Data,
            //        TotalRecordCount = result.TotalRecordCount
            //    });
            //}
            //catch (Exception)
            //{
            //    throw;
            //}

        });
    }
}
