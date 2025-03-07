using CFTenantPortal.Interfaces;
using CFTenantPortal.Models;

namespace CFTenantPortal.Services
{
    /// <summary>
    /// Bank payment gateway via Swift
    /// </summary>
    public class SwiftBankPaymentGatewayService : IBankPaymentGatewayService
    {
        public async Task<BankPaymentResponse> MakePaymentAsync(double amount, BankCard bankCard)
        {
            throw new NotImplementedException();
        }
    }
}
