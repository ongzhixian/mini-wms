using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Wms.Models.Bookstore
{
    public class Publisher
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string? Name { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? Country { get; set; }
    }
}
