using CFTenantPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFTenantPortal.Interfaces
{
    public interface IPaymentRequestService
    {
        Task CreateManagementFeesRequest(PropertyOwner propertyOwner, Property property, double amount);
    }
}
