using CFTenantPortal.Enums;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CFTenantPortal.Models
{
    /// <summary>
    /// Account transaction type. E.g. Management fees request
    /// </summary>
    public class AccountTransactionType
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;

        public string Description { get; set; } = String.Empty;        

        /// <summary>
        /// Transaction type enum. Typically for finding a specific AccountTransactionType instance where the Id
        /// isn't known
        /// </summary>
        public AccountTransactionTypes TransactionType { get; set; }
    }
}
