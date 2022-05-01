using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Wms.Models.Data.Bookstore
{
    public class Employee
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public char? Minit { get; set; }

        public string? JobId { get; set; }

        public int? JobLevel { get; set; }

        public string? PublisherId { get; set; }

        public DateTime? HireDate { get; set; }

    }
}
