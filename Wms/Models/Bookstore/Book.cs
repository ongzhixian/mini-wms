using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Wms.Models.Data.Bookstore
{
    public class Book
    {
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Title { get; set; } = null!;

        public decimal Price { get; set; }

        public string Category { get; set; } = null!;

        public string Author { get; set; } = null!;
    }
}
