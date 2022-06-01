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
        SetupAuthorSearch(app);
        SetupDanaBot(app);
    }

    private static void SetupDanaBot(WebApplication app)
    {
        app.MapPost("/api/danaUpdate", (Models.Telegram.Update updates) =>
        {
            Console.WriteLine("updates, {0}", updates);

            using (StreamWriter sw = new StreamWriter("C:/home/LogFiles/sample.txt", true))
            {
                sw.AutoFlush = true;
                sw.WriteLine("Received some message");
            }
        });
    }

    private static void SetupAuthorSearch(WebApplication app)
    {
        app.MapGet("/api/search/author", async (string? q, BookstoreContext bookstoreContext) =>
        {
            var filterBuilder = Builders<Author>.Filter;

            var options = new FindOptions<Author>
            {
                Skip = 0,
                Limit = 10,
                Sort = Builders<Author>.Sort.Ascending(m => m.LastName)
            };

            System.Text.RegularExpressions.Regex startWith =
                new System.Text.RegularExpressions.Regex(
                    $"^{q}.*",
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            PipelineDefinition<Author, Author> pipeline = new EmptyPipelineDefinition<Author>();

            pipeline.Match(a => startWith.IsMatch($"{a.FirstName} {a.LastName}"));

            // Working
            var result = await bookstoreContext.Authors
                .Aggregate()
                .Project(m => new
                {
                    fullname = m.FirstName + " " + m.LastName
                })
                .Match(a => startWith.IsMatch(a.fullname))
                .ToListAsync();

            return Results.Ok(
                JsonSerializer.Serialize(
                    result.Select(m => m.fullname)));

        });
    }

    private static void SetupCountrySearch(WebApplication app)
    {
        app.MapGet("/api/search/country", async (string? q, BookstoreContext bookstoreContext) =>
        {
            var filterBuilder = Builders<Country>.Filter;

            var options = new FindOptions<Country>
            {
                Skip = 0,
                Limit = 10,
                Sort = Builders<Country>.Sort.Ascending(m => m.Name)
            };

            var rg = new BsonRegularExpression($"/.*{q}.*/i");

            FilterDefinition<Country> filter;

            if (q == null)
            {
                filter = filterBuilder.Empty;
            }
            else
            {
                filter = filterBuilder.Regex(
                    m => m.Name, 
                    new BsonRegularExpression($"/.*{q}.*/i"));
            }

            IAsyncCursor<Country> docCursor = await bookstoreContext.Countries.FindAsync(filter, options);

            var result = await docCursor.ToListAsync();
            
            return Results.Ok(
                JsonSerializer.Serialize(result.Select(m => m.Name))
                );
        });
    }


}
