using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFTenantPortal.Models
{
    /// <summary>
    /// Details of password reset
    /// </summary>
    public class PasswordReset
    {
        /// <summary>
        /// Unique Id. This value will be included in the URL in the 'reset password' email so that
        /// the app only processes the latest request.
        /// </summary>
        public string Id { get; set; } = String.Empty;

        public DateTimeOffset EmailSendDateTime { get; set; }

        /// <summary>
        /// Date and time when reset expires
        /// </summary>
        public DateTimeOffset ExpiryDateTime { get; set; }
    }
}
