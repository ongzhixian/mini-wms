using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Wms.Models.Data.Bookstore
{
    public class RoyaltySchedule
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Title { get; set; } = string.Empty;


        public int? LowRange { get; set; }

        public int? HighRange { get; set; }

        public int? Royalty { get; set; }



    }
}
