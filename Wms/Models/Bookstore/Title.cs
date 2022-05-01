using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Wms.Models.Data.Bookstore
{
    public class Title
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        //[BsonElement("Name")]
        public string BookTitle { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public decimal Advance { get; set; }

        public int? Royalty { get; set; }

        public int? YearToDateSales { get; set; }

        public string? Notes { get; set; }

        public DateTime? PublishedDate { get; set; }
    }
}
