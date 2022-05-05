using MongoDB.Driver;
using Wms.DbContexts;
using Wms.Models.Bookstore;
using Wms.Models.Data.Bookstore;

namespace Wms.Services
{
    public class BookService
    {
        private readonly BookstoreContext bookstoreContext;

        public BookService(BookstoreContext bookstoreContext)
        {
            this.bookstoreContext = bookstoreContext;
        }

        public async Task<List<Book>> GetAsync() =>
            await bookstoreContext.Books.Find(_ => true).ToListAsync();

        public async Task<Book?> GetAsync(string id) =>
            await bookstoreContext.Books.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Book newBook) =>
            await bookstoreContext.Books.InsertOneAsync(newBook);

        public async Task UpdateAsync(string id, Book updatedBook) =>
            await bookstoreContext.Books.ReplaceOneAsync(x => x.Id == id, updatedBook);

        public async Task RemoveAsync(string id) =>
            await bookstoreContext.Books.DeleteOneAsync(x => x.Id == id);
    }
}
