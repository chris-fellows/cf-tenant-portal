using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CFTenantPortal.Models
{
    /// <summary>
    /// Account transaction related to property. E.g. Payment of management fees.
    /// </summary>
    public class AccountTransaction
    {
        /// <summary>
        /// Unique Id
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;

        /// <summary>
        /// Unique reference for use on correspondence
        /// </summary>
        public string Reference { get; set; } = String.Empty;

        /// <summary>
        /// Transaction type
        /// </summary>
        public string TypeId { get; set; } = String.Empty;

        /// <summary>
        /// Property associated with transaction
        /// </summary>
        public string PropertyId { get; set; } = String.Empty;

        /// <summary>
        /// Transaction amount
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Created date and time
        /// </summary>
        public DateTimeOffset CreatedDateTime { get; set; }
    }
}
