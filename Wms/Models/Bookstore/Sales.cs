using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Wms.Models.Data.Bookstore
{
    public class Sales
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Title { get; set; } = string.Empty;


        public string? OrderNumber { get; set; }
        public DateTime? OrderDate { get; set; }
        public int? Quantity { get; set; }

        public string? PaymentTerms { get; set; }





    }
}
