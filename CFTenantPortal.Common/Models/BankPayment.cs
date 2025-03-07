using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CFTenantPortal.Models
{
    /// <summary>
    /// Bank payment details
    /// </summary>
    public class BankPayment
    {
        /// <summary>
        /// Unique Id
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;

        public string CardAccountName { get; set; } = String.Empty;
        
        public string CardNumber { get; set; } = String.Empty;

        public string CardExpiry { get; set; } = String.Empty;

        public string CardCode { get; set; } = String.Empty;

        public bool Authorised { get; set; }

        public string ResponseCode { get; set; } = String.Empty;

        public DateTimeOffset CreatedDateTime { get; set; } = DateTimeOffset.Now;
    }
}
