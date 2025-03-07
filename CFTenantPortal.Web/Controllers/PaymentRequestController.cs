using CFTenantPortal.Models;
using Microsoft.AspNetCore.Mvc;

namespace CFTenantPortal.Controllers
{
    public class PaymentRequestController : Controller
    {
        public IActionResult ManagementFees(string propertyId)
        {
            var model = new ManagementFeesRequestVM()
            {
                PropertyId = propertyId
            };

            return View(model);
        }
    }
}
