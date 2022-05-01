using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Wms.Models.Data.Bookstore
{
    public class Discount
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string StoreId { get; set; } = string.Empty;

        public int? LowQuantity { get; set; }

        public int? HighQuantity { get; set; }

        public decimal? DiscountAmount { get; set; }



    }
}
