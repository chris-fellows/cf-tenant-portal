using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFTenantPortal.Models
{
    /// <summary>
    /// Account period. This is for annual management fees
    /// 
    /// Period start:
    /// - Create AccountPeriod for the year.
    /// - Create AccountTransaction (Request for property payment) for each property.
    /// - We need to track whether the payment request has been sent.
    /// 
    /// Property payment received:
    /// - Create AccountTransaction (Property payment) for property.
    /// 
    /// Reports that would be useful:
    /// - Property list where payment for period X.
    /// - Property list where no payment for period X.
    /// 
    /// Pages:
    /// - Create account period.
    /// </summary>
    public class AccountPeriod
    {   
        /// <summary>
        /// Unique Id
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;

        public DateTimeOffset StartDateTime { get; set; }

        public DateTimeOffset EndDateTime { get; set; }
    }
}
