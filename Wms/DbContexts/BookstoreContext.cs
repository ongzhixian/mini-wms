using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;
using Wms.Extensions;
using Wms.Models.Bookstore;

namespace Wms.DbContexts;

public class BookstoreContext
{
    public readonly IMongoCollection<Author> Authors;
    public readonly IMongoCollection<Category> Categories;
    public readonly IMongoCollection<Book> Books;

    public string DatabaseName { get; private set; }

    private readonly IMongoClient mongoClient;
    private readonly IMongoDatabase db;
    private IMemoryCache cache;

    public BookstoreContext(string connectionString, IMemoryCache cache)
    {
        mongoClient = new MongoClient(connectionString);
        DatabaseName = MongoUrl.Create(connectionString).DatabaseName;
        db = mongoClient.GetDatabase(DatabaseName);

        this.cache = cache;

        Authors = db.GetCollection<Author>(nameof(Author).ToCamelCase());
        Categories = db.GetCollection<Category>(nameof(Category).ToCamelCase());
        Books = db.GetCollection<Book>(nameof(Book).ToCamelCase());

        //this.Users = db.GetCollection<User>("Users");
        // services.AddSingleton<IMongoCollection<User>>(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<User>("user"));
        // services.AddSingleton<IMongoCollection<Bookmark>>(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<Bookmark>("bookmark"));
        // services.AddSingleton<IMongoCollection<Note>>(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<Note>("note"));
    }

    public async Task SetupIndexesAsync()
    {
        await Categories.SetupIndexesAsync();

        var t = new Address
        {
            Street = "6223 Bateman St.",
            Unit = string.Empty,
            City = "Berkeley",
            Region = "CA",
            PostCode = "94705",
            Country = "USA"
        };

        var tstring = t.ToString();
        var tstring2 = t.ToStringX();

        var aut = new Author
        {
            LastName = "Bennet",
            FirstName = "Abraham",
            Addresses = new List<Address>
                {
                    new Address
                    {
                        Street = "6223 Bateman St.",
                        Unit = string.Empty,
                        City = "Berkeley",
                        Region = "CA",
                        PostCode = "94705",
                        Country = "USA"
                    }
                },
            Contacts = new List<Contact>
                {
                    new Contact
                    {
                        ContactType = "Phone",
                        Value = "415 658-9932"
                    }
                },

        };

        //await SetupIndexesForCategoryAsync();
        //await SetupIndexesForBookAsync();
    }

    public async Task InitializeBookstoreAsync()
    {
        await Categories.SeedAsync(cache);
        
        //await SeedCategoriesAsync();
        await SeedAuthorsAsync();
        await SeedBooksAsync();
    }
    
    // METHODS

    private async Task<bool> CategoryExistsAsync(string category)
    {
        var filter = Builders<Category>.Filter.Eq(t => t.Name, category);

        return await Categories.CountDocumentsAsync(filter) > 0;
    }

    private async Task InsertCategoryIfNotExistsAsync(string category)
    {
        if (!await CategoryExistsAsync(category))
        {
            await Categories.InsertOneAsync(new Category { Name = category });
        }

        // Code for Upsert (for reference)
        //var filter = Builders<Category>.Filter.Eq(t => t.Name, category);
        //var updateDefinition = Builders<Category>.Update
        //        .Set(m => m.Name, category);
        //var updateResult = await Categories.UpdateOneAsync(filter, updateDefinition, new UpdateOptions { IsUpsert = true });

        // We are not using upsert because we are following the principle of not touching the data more than we have to.
        // Since what we want to do is add if it does not exists in database;
        // Let's not upsert just for the sake of convenience.
    }

    private async Task<Category?> GetCategoryIfExistsAsync(string category)
    {
        string cacheKey = $"bookstore-{nameof(Category)}-{category}";

        if (cache.TryGetValue(cacheKey, out Category cachedValue))
        {
            return cachedValue;
        }

        if (!await CategoryExistsAsync(category))
        {
            return default;
        }

        var filter = Builders<Category>.Filter.Eq(t => t.Name, category);

        var find = await Categories.FindAsync(filter);

        Category? result = await find.FirstOrDefaultAsync();

        if (result != null)
        {
            cache.Set(cacheKey, result, new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromHours(1)
            });
        }

        return result;
    }

    // GetCategoryIdIfExistsElseAdd
    // AddOrGetExistingCategory

    private async Task zzzGetCategoryAsync(string category)
    {
        // Get Category Id
        var filter = Builders<Category>.Filter.Eq(t => t.Name, category);
        var count = await Categories.CountDocumentsAsync(filter);

        if (count > 1)
        {
            throw new Exception("Too many records matching criteria found");
        }

        if (count == 0)
        {
            throw new Exception("No records matching criteria found");
        }

        var find = await Categories.FindAsync(filter);
        Category? res = await find.FirstOrDefaultAsync();


        //    var asyncCursor = await Categories.FindAsync(filter);
        //var list = await asyncCursor.ToListAsync();

        //var count = await Categories.CountDocumentsAsync(filter);


        //Task<Category>? www = await x.FirstOrDefaultAsync();
    }

    // SEED

    private async Task SeedBooksAsync()
    {
        // title ; cat ; lname ; fname
        var normalizedData = @"
Secrets of Silicon Valley;computing;Dull;Ann
Secrets of Silicon Valley;computing;Hunter;Sheryl
The Busy Executive's Database Guide;business;Bennet;Abraham
The Busy Executive's Database Guide;business;Green;Marjorie
";
        var entries = normalizedData.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);

        foreach (var item in entries)
        {
            var part = item.Split(';', StringSplitOptions.RemoveEmptyEntries);

            string title = part[0];
            string category = part[1];
            string lastName = part[2];
            string firstName = part[3];

            // Insert Category if not exists
            // Get Category Id
            var filter = Builders<Category>.Filter.Eq(t => t.Name, category);
            IFindFluent<Category, Category>? x = Categories.Find(filter);


            // Insert Category if not exists
            // Get Category Id
            await InsertCategoryIfNotExistsAsync(category);
            var a = await GetCategoryIfExistsAsync(category);
        }

        if (await Books.CountDocumentsAsync(_ => true) > 0)
        {
            return;
        }
        // title                                    category
        //'Secrets of Silicon Valley'               'popular_comp'
        //427-17-2319  Dull Ann
        //846-92-7186  Hunter Sheryl

        //'The Busy Executive''s Database Guide'    'business'

        // 
        // 

        //var books = new List<Book>
        //{
        //    new Book
        //    {
        //        Title = "asd", Authors = 
        //    }
        //};

        //await Books.InsertManyAsync(books);
    }

    private async Task SeedAuthorsAsync()
    {
        if (await Authors.CountDocumentsAsync(_ => true) > 0)
        {
            return;
        }

        var authors = new List<Author>
        {
            new Author
            {
                LastName = "Bennet",
                FirstName = "Abraham",
                Addresses = new List<Address>
                {
                    new Address
                    {
                        Street = "6223 Bateman St.",
                        Unit = string.Empty,
                        City = "Berkeley",
                        Region = "CA",
                        PostCode = "94705",
                        Country = "USA"
                    }
                },
                Contacts = new List<Contact>
                {
                    new Contact
                    {
                        ContactType = "Phone",
                        Value = "415 658-9932"
                    }
                },
                
            }
            //                  au_id          	au_lname       	au_fname       	phone	address	city	state	zip	contract
            //new Author { Hash = "409-56-7008", LastName = "Bennet", FirstName = "Abraham", Phone = "415 658-9932", Address = "6223 Bateman St.", City = "Berkeley", State = "CA", Zip = "94705", Contract = true },
            //new Author { Hash = "213-46-8915", LastName = "Green", FirstName = "Marjorie", Phone = "415 986-7020", Address = "309 63rd St. #411", City = "Oakland", State = "CA", Zip = "94618", Contract = true },
            //new Author { Hash = "238-95-7766", LastName = "Carson", FirstName = "Cheryl", Phone = "415 548-7723", Address = "589 Darwin Ln.", City = "Berkeley", State = "CA", Zip = "94705", Contract = true },
            //new Author { Hash = "998-72-3567", LastName = "Ringer", FirstName = "Albert", Phone = "801 826-0752", Address = "67 Seventh Av.", City = "Salt Lake City", State = "UT", Zip = "84152", Contract = true },
            //new Author { Hash = "899-46-2035", LastName = "Ringer", FirstName = "Anne", Phone = "801 826-0752", Address = "67 Seventh Av.", City = "Salt Lake City", State = "UT", Zip = "84152", Contract = true },
            //new Author { Hash = "722-51-5454", LastName = "DeFrance", FirstName = "Michel", Phone = "219 547-9982", Address = "3 Balding Pl.", City = "Gary", State = "IN", Zip = "46403", Contract = true },
            //new Author { Hash = "807-91-6654", LastName = "Panteley", FirstName = "Sylvia", Phone = "301 946-8853", Address = "1956 Arlington Pl.", City = "Rockville", State = "MD", Zip = "20853", Contract = true },
            //new Author { Hash = "893-72-1158", LastName = "McBadden", FirstName = "Heather", Phone = "707 448-4982", Address = "301 Putnam", City = "Vacaville", State = "CA", Zip = "95688", Contract = true },
            //new Author { Hash = "724-08-9931", LastName = "Stringer", FirstName = "Dirk", Phone = "415 843-2991", Address = "5420 Telegraph Av.", City = "Oakland", State = "CA", Zip = "94609", Contract = false },
            //new Author { Hash = "274-80-9391", LastName = "Straight", FirstName = "Dean", Phone = "415 834-2919", Address = "5420 College Av.", City = "Oakland", State = "CA", Zip = "94609", Contract = false },
            //new Author { Hash = "756-30-7391", LastName = "Karsen", FirstName = "Livia", Phone = "415 534-9219", Address = "5720 McAuley St.", City = "Oakland", State = "CA", Zip = "94609", Contract = true },
            //new Author { Hash = "724-80-9391", LastName = "MacFeather", FirstName = "Stearns", Phone = "415 354-7128", Address = "44 Upland Hts.", City = "Oakland", State = "CA", Zip = "94612", Contract = true },
            //new Author { Hash = "427-17-2319", LastName = "Dull", FirstName = "Ann", Phone = "415 836-7128", Address = "3410 Blonde St.", City = "Palo Alto", State = "CA", Zip = "94301", Contract = true },
            //new Author { Hash = "672-71-3249", LastName = "Yokomoto", FirstName = "Akiko", Phone = "415 935-4228", Address = "3 Silver Ct.", City = "Walnut Creek", State = "CA", Zip = "94595", Contract = true },
            //new Author { Hash = "267-41-2394", LastName = "OLeary", FirstName = "Michael", Phone = "408 286-2428", Address = "22 Cleveland Av. #14", City = "San Jose", State = "CA", Zip = "95128", Contract = true },
            //new Author { Hash = "472-27-2349", LastName = "Gringlesby", FirstName = "Burt", Phone = "707 938-6445", Address = "PO Box 792", City = "Covelo", State = "CA", Zip = "95428", Contract = true },
            //new Author { Hash = "527-72-3246", LastName = "Greene", FirstName = "Morningstar", Phone = "615 297-2723", Address = "22 Graybar House Rd.", City = "Nashville", State = "TN", Zip = "37215", Contract = true },
            //new Author { Hash = "172-32-1176", LastName = "White", FirstName = "Johnson", Phone = "408 496-7223", Address = "10932 Bigge Rd.", City = "Menlo Park", State = "CA", Zip = "94025", Contract = true },
            //new Author { Hash = "712-45-1867", LastName = "del Castillo", FirstName = "Innes", Phone = "615 996-8275", Address = "2286 Cram Pl. #86", City = "Ann Arbor", State = "MI", Zip = "48105", Contract = true },
            //new Author { Hash = "846-92-7186", LastName = "Hunter", FirstName = "Sheryl", Phone = "415 836-7128", Address = "3410 Blonde St.", City = "Palo Alto", State = "CA", Zip = "94301", Contract = true },
            //new Author { Hash = "486-29-1786", LastName = "Locksley", FirstName = "Charlene", Phone = "415 585-4620", Address = "18 Broadway Av.", City = "San Francisco", State = "CA", Zip = "94130", Contract = true },
            //new Author { Hash = "648-92-1872", LastName = "Blotchet-Halls", FirstName =  "Reginald", Phone = "503 745-6402", Address = "55 Hillsdale Bl.", City = "Corvallis", State = "OR", Zip = "97330", Contract = true },
            //new Author { Hash = "341-22-1782", LastName = "Smith", FirstName = "Meander", Phone = "913 843-0462", Address = "10 Mississippi Dr.", City = "Lawrence", State = "KS", Zip = "66044", Contract = true },

        };

        await Authors.InsertManyAsync(authors);
    }

    //private async Task SeedCategoriesAsync()
    //{
    //    if (await Categories.CountDocumentsAsync(_ => true) > 0)
    //    {
    //        return;
    //    }

    //    var categories = new List<Category>
    //    {
    //        new Category { Name = "business" },
    //        new Category { Name = "psychology" },
    //        new Category { Name = "traditional_cooking" },
    //        new Category { Name = "modern-cooking" },
    //        new Category { Name = "computing" },
    //    };

    //    await Categories.InsertManyAsync(categories);

    //    //Categories.SeedAsync(cache);
    //}

    // SETUP INDEXES



    //private async Task SetupIndexesForBookAsync()
    //{
    //    try
    //    {
    //        var documentCursor = await Books.Indexes.ListAsync();

    //        if (documentCursor != null)
    //        {
    //            var indexes = await documentCursor.ToListAsync();

    //            if (indexes.Count <= 1)
    //            {
    //                var indexKeys = Builders<Book>.IndexKeys
    //                    .Ascending(m => m.Title)
    //                    .Ascending(m => m.Author);

    //                var indexOptions = new CreateIndexOptions { Unique = true };

    //                await Books.Indexes.CreateOneAsync(new CreateIndexModel<Book>(indexKeys, indexOptions));
    //            }
    //        }
    //    }
    //    catch (TimeoutException)
    //    {
    //        Console.WriteLine("Timeout on SetupIndexesForBookAsync; MongoDb maybe unavailable.");
    //    }
    //}

    //private async Task SetupIndexesForAuthorAsync()
    //{
    //    try
    //    {
    //        var documentCursor = await Authors.Indexes.ListAsync();

    //        if (documentCursor != null)
    //        {
    //            var indexes = await documentCursor.ToListAsync();

    //            if (indexes.Count <= 1)
    //            {
    //                var indexKeys = Builders<Author>.IndexKeys
    //                    .Ascending(m => m.)
    //                    .Ascending(m => m.Author);

    //                var indexOptions = new CreateIndexOptions { Unique = true };

    //                await Books.Indexes.CreateOneAsync(new CreateIndexModel<Book>(indexKeys, indexOptions));
    //            }
    //        }
    //    }
    //    catch (TimeoutException)
    //    {
    //        Console.WriteLine("Timeout on SetupIndexesForBookAsync; MongoDb maybe unavailable.");
    //    }
    //}
}