using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Wms.Models.Bookstore
{
    public class Author
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string? Hash { get; set; }

        public string? LastName { get; set; }

        public string? FirstName { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }
        
        public string? State { get; set; }
        
        public string? Zip { get; set; }

        public bool? Contract { get; set; }

    }
}
