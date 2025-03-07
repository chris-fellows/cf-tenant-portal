using CFTenantPortal.Models;

namespace CFTenantPortal.Interfaces
{
    public interface IBankPaymentGatewayService
    {
        Task<BankPaymentResponse> MakePaymentAsync(double amount, BankCard bankCard);
    }
}
