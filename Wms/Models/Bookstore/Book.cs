namespace Wms.Models.Bookstore
{
    public class Book
    {
        public string? Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public IList<Category> Categories { get; set; } = new List<Category>();

        public IList<Author> Authors { get; set; } = new List<Author>();
    }
}
