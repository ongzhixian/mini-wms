using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Wms.Models.Data.Bookstore
{
    public class Job
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Description { get; set; } = string.Empty;


        public int? MinimumLevel { get; set; }

        public int? MaximumLevel { get; set; }

    }
}
