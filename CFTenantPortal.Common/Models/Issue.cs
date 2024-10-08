﻿using CFTenantPortal.Enums;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CFTenantPortal.Models
{
    /// <summary>
    /// Issue related to property or property group    
    /// </summary>
    public class Issue
    {
        /// <summary>
        /// Unique Id
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;

        /// <summary>
        /// Unique reference. This is publicly visible and would be quoted in communications.
        /// </summary>
        public string Reference { get; set; } = String.Empty;

        /// <summary>
        /// Issue type
        /// </summary>
        public string TypeId { get; set; } = String.Empty;

        /// <summary>
        /// Property (if any) related to issue
        /// </summary>
        public string PropertyId { get; set; } = String.Empty;

        /// <summary>
        /// Property group (if any) related to issue
        /// </summary>
        public string PropertyGroupId { get; set; } = String.Empty;

        /// <summary>
        /// Issue description
        /// </summary>
        public string Description { get; set; } = String.Empty;

        /// <summary>
        /// Issue status
        /// </summary>
        public string StatusId { get; set; } = String.Empty;

        /// <summary>
        /// Employee (if any) that created issue
        /// </summary>
        public string CreatedEmployeeId { get; set; } = String.Empty;

        /// <summary>
        /// Property owner (if any) that created issue
        /// </summary>
        public string CreatedPropertyOwnerId { get; set; } = String.Empty;

        /// <summary>
        /// Created date and time
        /// </summary>
        public DateTimeOffset CreatedDateTime { get; set; } = DateTimeOffset.Now;

        /// <summary>
        /// Documents associated with issue
        /// </summary>
        public List<string> DocumentIds { get; set; } = new List<string>();
    }
}
