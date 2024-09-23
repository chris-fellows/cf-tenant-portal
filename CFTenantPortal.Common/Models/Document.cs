using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CFTenantPortal.Models
{
    /// <summary>
    /// Document. Relates to issue, message (E.g. Invoice for management fees)
    /// </summary>
    public class Document
    {
        /// <summary>
        /// Unique Id
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;        

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; } = String.Empty;

        /// <summary>
        /// Document content
        /// </summary>
        public byte[] Content { get; set; } = new byte[0];
    }
}
