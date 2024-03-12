using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace StocksPortfolio.Infrastructure.Data.Model
{
    public class Portfolio
    {
        [BsonElement("id")]
        public ObjectId Id { get; set; }

        [BsonElement("totalValue")]
        public float CurrentTotalValue { get; set; }

        [BsonElement("stocks")]
        public ICollection<Stock> Stocks { get; set; }

        [BsonElement("deletedOnUtc")]
        public DateTime? DeletedOnUtc { get; set; }
    }
}
