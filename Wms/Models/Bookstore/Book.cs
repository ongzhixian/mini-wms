using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Wms.Models.Data.Bookstore
{
    public class Book
    {
        public string? Id { get; set; }

        public string Title { get; set; } = null!;

        public string Category { get; set; } = null!;

        public string Author { get; set; } = null!;
    }
}
