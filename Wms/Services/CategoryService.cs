using MongoDB.Driver;
using Wms.DbContexts;
using Wms.Models.Data.Bookstore;

namespace Wms.Services
{
    public class CategoryService
    {
        private readonly BookstoreContext bookstoreContext;

        public CategoryService(BookstoreContext bookstoreContext)
        {
            this.bookstoreContext = bookstoreContext;
        }

        public async Task<List<Category>> GetAsync() =>
            await bookstoreContext.Categories.Find(_ => true).ToListAsync();

        public async Task<Category?> GetAsync(string id) =>
            await bookstoreContext.Categories.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Category newCategory) =>
            await bookstoreContext.Categories.InsertOneAsync(newCategory);

        public async Task UpdateAsync(string id, Category updatedCategory) =>
            await bookstoreContext.Categories.ReplaceOneAsync(x => x.Id == id, updatedCategory);

        public async Task RemoveAsync(string id) =>
            await bookstoreContext.Categories.DeleteOneAsync(x => x.Id == id);
    }
}
