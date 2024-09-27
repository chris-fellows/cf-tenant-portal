using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CFTenantPortal.Models
{
    /// <summary>
    /// Property owner
    /// </summary>
    public class PropertyOwner
    {
        /// <summary>
        /// Unique Id
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;

        /// <summary>
        /// Owner name
        /// </summary>
        public string Name { get; set; } = String.Empty;

        /// <summary>
        /// Email address
        /// </summary>
        public string Email { get; set; } = String.Empty;

        /// <summary>
        /// Phone number
        /// </summary>
        public string Phone { get; set; } = String.Empty;

        /// <summary>
        /// Address for communications
        /// </summary>
        public Address Address { get; set; } = new Address();

        /// <summary>
        /// Documents associated with property owner
        /// </summary>
        public List<string> DocumentIds { get; set; } = new List<string>();

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; } = String.Empty;

        /// <summary>
        /// Password reset details if reset requested
        /// </summary>
        public PasswordReset? PasswordReset { get; set; }
    }
}
